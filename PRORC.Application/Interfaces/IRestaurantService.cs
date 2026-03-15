using PRORC.Application.DTOs.Restaurants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Application.Interfaces
{
    public interface IRestaurantService
    {
        Task<RestaurantDto?> GetByIdAsync(int id);
        Task<List<RestaurantDto>> GetActiveAsync();
        Task<List<RestaurantDto>> SearchAsync(string search);
        Task<RestaurantDto> CreateAsync(CreateRestaurantRequest request);
    }
}
