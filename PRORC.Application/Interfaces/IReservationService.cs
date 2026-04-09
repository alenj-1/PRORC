using PRORC.Application.DTOs.Reservations;

namespace PRORC.Application.Interfaces
{
    public interface IReservationService
    {
        Task<ReservationDto> CreateReservationAsync(CreateReservationRequest request);
        Task<ReservationDto?> GetByIdAsync(int reservationId);
        Task<IEnumerable<ReservationDto>> GetByUserIdAsync(int userId);
        Task<IEnumerable<ReservationDto>> GetByRestaurantIdAsync(int restaurantId);
        Task ConfirmReservationAsync(int reservationId);
        Task CompleteReservationAsync(int reservationId);
        Task CancelReservationAsync(int reservationId);
    }
}