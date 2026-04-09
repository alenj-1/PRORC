using PRORC.Application.DTOs.Reviews;

namespace PRORC.Application.Interfaces
{
    public interface IReviewService
    {
        Task<ReviewDto> CreateReviewAsync(CreateReviewRequest request);
        Task<IEnumerable<ReviewDto>> GetByRestaurantIdAsync(int restaurantId);
        Task<IEnumerable<ReviewDto>> GetByUserIdAsync(int userId);
    }
}