using PRORC.Application.DTOs.Reservations;
using PRORC.Application.Interfaces;
using PRORC.Domain.Entities.Reservations;
using PRORC.Domain.Enums;
using PRORC.Domain.Interfaces.Repositories;

namespace PRORC.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRestaurantRepository _restaurantRepository;

        public ReservationService(
            IReservationRepository reservationRepository,
            IUserRepository userRepository,
            IRestaurantRepository restaurantRepository)
        {
            _reservationRepository = reservationRepository;
            _userRepository = userRepository;
            _restaurantRepository = restaurantRepository;
        }

        public async Task<ReservationDto> CreateAsync(CreateReservationRequest request)
        {
            if (await _userRepository.GetByIdAsync(request.UserId) is null)
                throw new InvalidOperationException("The user does not exist.");

            if (await _restaurantRepository.GetByIdAsync(request.RestaurantId) is null)
                throw new InvalidOperationException("The restaurant does not exist.");

            if (request.PartySize <= 0)
                throw new InvalidOperationException("Group size must be greater than zero.");

            if (request.ReservationDate <= DateTime.Now)
                throw new InvalidOperationException("Reservation must be for a future date.");

            var hasConflict = await _reservationRepository.HasReservationConflictAsync(
                request.RestaurantId,
                request.ReservationDate);

            if (hasConflict)
                throw new InvalidOperationException("A reservation has already been made for that date and restaurant.");

            var entity = new Reservation
            {
                UserId = request.UserId,
                RestaurantId = request.RestaurantId,
                ReservationDate = request.ReservationDate,
                PartySize = request.PartySize,
                Status = ReservationStatusEnum.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _reservationRepository.AddAsync(entity);
            return Map(entity);
        }

        public async Task<List<ReservationDto>> GetByUserAsync(int userId)
        {
            var reservations = await _reservationRepository.GetReservationsByUserAsync(userId);
            return reservations.Select(Map).ToList();
        }

        public async Task<List<ReservationDto>> GetByRestaurantAsync(int restaurantId)
        {
            var reservations = await _reservationRepository.GetReservationsByRestaurantAsync(restaurantId);
            return reservations.Select(Map).ToList();
        }

        private static ReservationDto Map(Reservation reservation) => new()
        {
            Id = reservation.Id,
            UserId = reservation.UserId,
            RestaurantId = reservation.RestaurantId,
            ReservationDate = reservation.ReservationDate,
            PartySize = reservation.PartySize,
            Status = reservation.Status,
            CreatedAt = reservation.CreatedAt
        };
    }
}
