using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRORC.Api.Security;
using PRORC.Application.DTOs.Reviews;
using PRORC.Application.Interfaces;
using System.Security.Claims;

namespace PRORC.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class ReviewsController(IReviewService reviewService) : ControllerBase
    {
        private readonly IReviewService _reviewService = reviewService;

        // POST que permite crear una reseña nueva
        [Authorize(Policy = AuthPolicies.CustomerOnly)]
        [HttpPost]
        public async Task<ActionResult<ReviewDto>> Create([FromBody] CreateReviewRequest request)
        {
            var review = await _reviewService.CreateAsync(request);
            return Ok(review);
        }

        // GET que permite obtener las reseñas de un restaurante
        [AllowAnonymous]
        [HttpGet("restaurant/{restaurantId:int}")]
        public async Task<ActionResult<List<ReviewDto>>> GetByRestaurant(int restaurantId)
        {
            var reviews = await _reviewService.GetByRestaurantAsync(restaurantId);
            return Ok(reviews);
        }

        // GET que permite obtener las reseñas creadas por un usuario
        [Authorize]
        [HttpGet("user/{userId:int}")]
        public async Task<ActionResult<List<ReviewDto>>> GetByUser(int userId)
        {
            if (!CanAccessUserData(userId))
            {
                return Forbid();
            }

            var reviews = await _reviewService.GetByUserAsync(userId);
            return Ok(reviews);
        }

        // GET que devuelve el promedio de calificación de un restaurante
        [AllowAnonymous]
        [HttpGet("restaurant/{restaurantId:int}/average-rating")]
        public async Task<ActionResult<double>> GetAverageRating(int restaurantId)
        {
            var average = await _reviewService.GetAverageRatingAsync(restaurantId);
            return Ok(average);
        }


        // Método para evitar que un usuario que no tenga rol de SystemAdmin consulte los datos de otro usuario
        private bool CanAccessUserData(int userId)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (role == "SystemAdmin")
            {
                return true;
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return false;
            }

            if (!int.TryParse(userIdClaim, out int currentUserId))
            {
                return false;
            }

            return currentUserId == userId;
        }
    }
}
