using PRORC.Domain.Entities.Reservations;
using PRORC.Domain.Enums;

namespace PRORC.Domain.Interfaces.Repositories
{
    public interface IReservationRepository : IBaseRepository<Reservation>
    {
        Task<IEnumerable<Reservation>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Reservation>> GetByRestaurantIdAsync(int restaurantId);
        Task<IEnumerable<Reservation>> GetByDateAsync(int restaurantId, DateTime reservationDate);
        Task<IEnumerable<Reservation>> GetByStatusAsync(ReservationStatus status);
    }
}