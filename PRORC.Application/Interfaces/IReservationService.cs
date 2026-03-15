using PRORC.Application.DTOs.Reservations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Application.Interfaces
{
    public interface IReservationService
    {
        Task<ReservationDto> CreateAsync(CreateReservationRequest request);
        Task<List<ReservationDto>> GetByUserAsync(int userId);
        Task<List<ReservationDto>> GetByRestaurantAsync(int restaurantId);
    }
}
