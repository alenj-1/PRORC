using Microsoft.AspNetCore.Mvc;
using PRORC.Application.DTOs.Reviews;
using PRORC.Application.Interfaces;

namespace PRORC.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("restaurant/{restaurantId:int}")]
        public async Task<IActionResult> GetByRestaurant(int restaurantId)
        {
            var result = await _reviewService.GetByRestaurantAsync(restaurantId);
            return Ok(result);
        }

        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var result = await _reviewService.GetByUserAsync(userId);
            return Ok(result);
        }

        [HttpGet("restaurant/{restaurantId:int}/average")]
        public async Task<IActionResult> GetAverageRating(int restaurantId)
        {
            var result = await _reviewService.GetAverageRatingAsync(restaurantId);
            return Ok(new { restaurantId, averageRating = result });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReviewRequest request)
        {
            try
            {
                var result = await _reviewService.CreateAsync(request);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
