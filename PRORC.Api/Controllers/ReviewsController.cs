using Microsoft.AspNetCore.Mvc;
using PRORC.Application.DTOs.Reviews;
using PRORC.Application.Interfaces;

namespace PRORC.Api.Controllers
{
    [ApiController]
    [Route("api/reviews")]

    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }


        // POST que permite crear una reseña
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReviewRequest request)
        {
            var result = await _reviewService.CreateReviewAsync(request);
            return Ok(result);
        }


        // GET que permite obtener reseñas por restaurante
        [HttpGet("restaurant/{restaurantId:int}")]
        public async Task<IActionResult> GetByRestaurantId(int restaurantId)
        {
            var result = await _reviewService.GetByRestaurantIdAsync(restaurantId);
            return Ok(result);
        }


        // GET que permite obtener reseñas por usuario
        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var result = await _reviewService.GetByUserIdAsync(userId);
            return Ok(result);
        }
    }
}