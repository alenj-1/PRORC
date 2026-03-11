using PRORC.Domain.Entities.Reservations;
using PRORC.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Domain.Interfaces.Repositories
{
    public interface IReservationRepository : IBaseRepository<Reservation, int>
    {
        Task<List<Reservation>> GetReservationsByUserAsync(int userId);
        Task<List<Reservation>> GetReservationsByRestaurantAsync(int restaurantId);
        Task<bool> HasReservationConflictAsync(int restaurantId, DateTime reservationDate);
        Task<List<Reservation>> GetReservationsByStatusAsync(ReservationStatusEnum status);
        Task<List<Reservation>> GetReservationsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<Reservation>> GetUpcomingReservationsByUserAsync(int userId);
        Task<List<Reservation>> GetUpcomingReservationsByRestaurantAsync(int restaurantId);
        Task<bool> ExistsReservationForUserAsync(int reservationId, int userId);
    }
}
