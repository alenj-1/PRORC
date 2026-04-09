using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PRORC.Domain.Entities.Reservations;
using PRORC.Domain.Enums;
using PRORC.Domain.Interfaces.Repositories;
using PRORC.Persistence.Base;
using PRORC.Persistence.Context;

namespace PRORC.Persistence.Repositories.Reservations
{
    public class ReservationRepository : BaseRepository<Reservation>, IReservationRepository
    {
        public ReservationRepository(PRORCContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory) { }

        public async Task<IEnumerable<Reservation>> GetByUserIdAsync(int userId)
        {
            try
            {
                return await _context.Reservations
                    .AsNoTracking()
                    .Where(r => r.UserId == userId)
                    .OrderByDescending(r => r.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting reservations by UserId {UserId}.", userId);
                throw;
            }
        }

        public async Task<IEnumerable<Reservation>> GetByRestaurantIdAsync(int restaurantId)
        {
            try
            {
                return await _context.Reservations
                    .AsNoTracking()
                    .Where(r => r.RestaurantId == restaurantId)
                    .OrderByDescending(r => r.ReservationDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting reservations by RestaurantId {RestaurantId}.", restaurantId);
                throw;
            }
        }

        public async Task<IEnumerable<Reservation>> GetByDateAsync(int restaurantId, DateTime reservationDate)
        {
            try
            {
                var onlyDate = reservationDate.Date;

                return await _context.Reservations
                    .AsNoTracking()
                    .Where(r => r.RestaurantId == restaurantId && r.ReservationDate.Date == onlyDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting reservations by RestaurantId {RestaurantId} and date {ReservationDate}.", restaurantId, reservationDate);
                throw;
            }
        }

        public async Task<IEnumerable<Reservation>> GetByStatusAsync(ReservationStatus status)
        {
            try
            {
                return await _context.Reservations
                    .AsNoTracking()
                    .Where(r => r.Status == status)
                    .OrderByDescending(r => r.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting reservations by Status {Status}.", status);
                throw;
            }
        }
    }
}