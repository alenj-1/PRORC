using Microsoft.Extensions.Logging;
using PRORC.Application.DTOs.Reservations;
using PRORC.Application.Interfaces;
using PRORC.Domain.Entities.Reservations;
using PRORC.Domain.Interfaces.Logging;
using PRORC.Domain.Interfaces.Repositories;

namespace PRORC.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IAuditLogger _auditLogger;
        private readonly ILogger<ReservationService> _logger;

        public ReservationService(
            IReservationRepository reservationRepository,
            IRestaurantRepository restaurantRepository,
            IAuditLogger auditLogger,
            ILogger<ReservationService> logger)
        {
            _reservationRepository = reservationRepository;
            _restaurantRepository = restaurantRepository;
            _auditLogger = auditLogger;
            _logger = logger;
        }

        public async Task<ReservationDto> CreateReservationAsync(CreateReservationRequest request)
        {
            try
            {
                if (request == null)
                    throw new ArgumentNullException(nameof(request));

                var restaurant = await _restaurantRepository.GetByIdAsync(request.RestaurantId)
                    ?? throw new KeyNotFoundException("Restaurant not found.");

                if (!restaurant.IsActive)
                    throw new InvalidOperationException("The restaurant is inactive.");

                var availability = await _restaurantRepository.GetAvailabilitySlotAsync(
                    request.RestaurantId,
                    request.ReservationDate,
                    request.ReservationTime);

                if (availability == null)
                    throw new InvalidOperationException("There is no availability for that date and time.");

                if (availability.AvailableTables < request.NumberOfTables)
                    throw new InvalidOperationException("There are not enough tables available.");

                var reservation = Reservation.Create(
                    request.UserId,
                    request.RestaurantId,
                    request.ReservationDate,
                    request.ReservationTime,
                    request.NumberOfTables);

                // Apartamos las mesas desde el momento en que se crea la reserva
                availability.ReserveTables(request.NumberOfTables);

                var createdReservation = await _reservationRepository.AddAsync(reservation);
                await _restaurantRepository.UpdateAvailabilityAsync(availability);

                _logger.LogInformation("Reservation {ReservationId} created successfully.", createdReservation.Id);

                await TryWriteAuditAsync(
                    createdReservation.UserId,
                    "CreateReservation",
                    "Reservation",
                    createdReservation.Id,
                    $"Reservation created for restaurant {createdReservation.RestaurantId}.");

                return MapReservation(createdReservation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating reservation for restaurant {RestaurantId}.", request?.RestaurantId);
                throw;
            }
        }

        public async Task<ReservationDto?> GetByIdAsync(int reservationId)
        {
            try
            {
                var reservation = await _reservationRepository.GetByIdAsync(reservationId);
                return reservation == null ? null : MapReservation(reservation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting reservation {ReservationId}.", reservationId);
                throw;
            }
        }

        public async Task<IEnumerable<ReservationDto>> GetByUserIdAsync(int userId)
        {
            try
            {
                var reservations = await _reservationRepository.GetByUserIdAsync(userId);
                return reservations.Select(MapReservation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting reservations for user {UserId}.", userId);
                throw;
            }
        }

        public async Task<IEnumerable<ReservationDto>> GetByRestaurantIdAsync(int restaurantId)
        {
            try
            {
                var reservations = await _reservationRepository.GetByRestaurantIdAsync(restaurantId);
                return reservations.Select(MapReservation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting reservations for restaurant {RestaurantId}.", restaurantId);
                throw;
            }
        }

        public async Task ConfirmReservationAsync(int reservationId)
        {
            try
            {
                var reservation = await _reservationRepository.GetByIdAsync(reservationId)
                    ?? throw new KeyNotFoundException("Reservation not found.");

                reservation.Confirm();

                await _reservationRepository.UpdateAsync(reservation);

                await TryWriteAuditAsync(reservation.UserId, "ConfirmReservation", "Reservation", reservation.Id, "Reservation confirmed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirming reservation {ReservationId}.", reservationId);
                throw;
            }
        }

        public async Task CompleteReservationAsync(int reservationId)
        {
            try
            {
                var reservation = await _reservationRepository.GetByIdAsync(reservationId)
                    ?? throw new KeyNotFoundException("Reservation not found.");

                reservation.Complete();

                await _reservationRepository.UpdateAsync(reservation);

                await TryWriteAuditAsync(reservation.UserId, "CompleteReservation", "Reservation", reservation.Id, "Reservation completed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing reservation {ReservationId}.", reservationId);
                throw;
            }
        }

        public async Task CancelReservationAsync(int reservationId)
        {
            try
            {
                var reservation = await _reservationRepository.GetByIdAsync(reservationId)
                    ?? throw new KeyNotFoundException("Reservation not found.");

                reservation.Cancel();

                await _reservationRepository.UpdateAsync(reservation);

                // Si conseguimos el slot, liberamos las mesas.
                // Si esto falla, no reventamos la cancelación principal.
                try
                {
                    var availability = await _restaurantRepository.GetAvailabilitySlotAsync(
                        reservation.RestaurantId,
                        reservation.ReservationDate,
                        reservation.ReservationTime);

                    if (availability != null)
                    {
                        availability.FreeTables(reservation.NumberOfTables);
                        await _restaurantRepository.UpdateAvailabilityAsync(availability);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Could not release tables after cancelling reservation {ReservationId}.", reservationId);
                }

                await TryWriteAuditAsync(reservation.UserId, "CancelReservation", "Reservation", reservation.Id, "Reservation cancelled.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling reservation {ReservationId}.", reservationId);
                throw;
            }
        }

        private ReservationDto MapReservation(Reservation reservation)
        {
            return new ReservationDto
            {
                Id = reservation.Id,
                UserId = reservation.UserId,
                RestaurantId = reservation.RestaurantId,
                ReservationDate = reservation.ReservationDate,
                ReservationTime = reservation.ReservationTime,
                NumberOfTables = reservation.NumberOfTables,
                Status = reservation.Status.ToString(),
                CreatedAt = reservation.CreatedAt
            };
        }

        private async Task TryWriteAuditAsync(int? userId, string action, string entityName, int entityId, string details)
        {
            try
            {
                await _auditLogger.LogAsync(userId, action, entityName, entityId, details);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Audit logging failed in ReservationService.");
            }
        }
    }
}