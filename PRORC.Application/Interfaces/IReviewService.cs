using PRORC.Application.DTOs.Reviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Application.Interfaces
{
    public interface IReviewService
    {
        Task<ReviewDto> CreateAsync(CreateReviewRequest request);
        Task<List<ReviewDto>> GetByRestaurantAsync(int restaurantId);
        Task<List<ReviewDto>> GetByUserAsync(int userId);
        Task<double> GetAverageRatingAsync(int restaurantId);
    }
}
