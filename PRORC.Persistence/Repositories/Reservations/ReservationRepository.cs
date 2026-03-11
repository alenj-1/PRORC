using Microsoft.EntityFrameworkCore;
using PRORC.Domain.Entities.Reservations;
using PRORC.Domain.Enums;
using PRORC.Domain.Interfaces.Repositories;
using PRORC.Persistence.Base;
using PRORC.Persistence.Context;

namespace PRORC.Persistence.Repositories.Reservations
{
    public class ReservationRepository : BaseRepository<Reservation, int>, IReservationRepository
    {
        public ReservationRepository(PRORCContext context) : base(context)
        {
        }

        public async Task<List<Reservation>> GetReservationsByUserAsync(int userId)
        {
            return await _dbSet
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.ReservationDate)
                .ToListAsync();
        }

        public async Task<List<Reservation>> GetReservationsByRestaurantAsync(int restaurantId)
        {
            return await _dbSet
                .Where(r => r.RestaurantId == restaurantId)
                .OrderByDescending(r => r.ReservationDate)
                .ToListAsync();
        }

        public async Task<bool> HasReservationConflictAsync(int restaurantId, DateTime reservationDate)
        {
            return await _dbSet
                .AnyAsync(r => r.RestaurantId == restaurantId &&
                               r.ReservationDate == reservationDate);
        }

        public async Task<List<Reservation>> GetReservationsByStatusAsync(ReservationStatusEnum status)
        {
            return await _dbSet
                .Where(r => r.Status == status)
                .OrderByDescending(r => r.ReservationDate)
                .ToListAsync();
        }

        public async Task<List<Reservation>> GetReservationsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(r => r.ReservationDate >= startDate && r.ReservationDate <= endDate)
                .OrderByDescending(r => r.ReservationDate)
                .ToListAsync();
        }

        public async Task<List<Reservation>> GetUpcomingReservationsByUserAsync(int userId)
        {
            var now = DateTime.Now;

            return await _dbSet
                .Where(r => r.UserId == userId && r.ReservationDate >= now)
                .OrderBy(r => r.ReservationDate)
                .ToListAsync();
        }

        public async Task<List<Reservation>> GetUpcomingReservationsByRestaurantAsync(int restaurantId)
        {
            var now = DateTime.Now;

            return await _dbSet
                .Where(r => r.RestaurantId == restaurantId && r.ReservationDate >= now)
                .OrderBy(r => r.ReservationDate)
                .ToListAsync();
        }

        public async Task<bool> ExistsReservationForUserAsync(int reservationId, int userId)
        {
            return await _dbSet
                .AnyAsync(r => r.Id == reservationId && r.UserId == userId);
        }
    }
}
