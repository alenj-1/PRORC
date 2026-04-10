using Microsoft.AspNetCore.Mvc;
using PRORC.Application.DTOs.Restaurants;
using PRORC.Application.Interfaces;

namespace PRORC.Api.Controllers
{
    [ApiController]
    [Route("api/restaurants")]

    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantsController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }


        // POST que permite crear un restaurante
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRestaurantRequest request)
        {
            var result = await _restaurantService.CreateRestaurantAsync(request);
            return Ok(result);
        }


        // GET que permite buscar un restaurante por Id
        [HttpGet("{restaurantId:int}")]
        public async Task<IActionResult> GetById(int restaurantId)
        {
            var result = await _restaurantService.GetByIdAsync(restaurantId);

            if (result == null)
                return NotFound(new { message = "Restaurant not found." });

            return Ok(result);
        }


        // GET que permite obtener los restaurantes de un propietario
        [HttpGet("owner/{ownerId:int}")]
        public async Task<IActionResult> GetByOwnerId(int ownerId)
        {
            var result = await _restaurantService.GetByOwnerIdAsync(ownerId);
            return Ok(result);
        }


        // GET que permite buscar restaurantes usando filtros opcionales
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? cuisineType, [FromQuery] string? address, [FromQuery] double? minimumRating)
        {
            var result = await _restaurantService.SearchAsync(cuisineType, address, minimumRating);
            return Ok(result);
        }


        // PATCH que permite activar un restaurante
        [HttpPatch("{restaurantId:int}/activate")]
        public async Task<IActionResult> Activate(int restaurantId)
        {
            await _restaurantService.ActivateRestaurantAsync(restaurantId);
            return Ok(new { message = "Restaurant activated successfully." });
        }


        // PATCH que permite desactivar un restaurante
        [HttpPatch("{restaurantId:int}/deactivate")]
        public async Task<IActionResult> Deactivate(int restaurantId)
        {
            await _restaurantService.DeactivateRestaurantAsync(restaurantId);
            return Ok(new { message = "Restaurant deactivated successfully." });
        }


        // POST que permite registrar disponibilidad para un restaurante
        [HttpPost("{restaurantId:int}/availability")]
        public async Task<IActionResult> SetAvailability(int restaurantId, [FromBody] SetAvailabilityApiRequest request)
        {
            await _restaurantService.SetAvailabilityAsync(
                restaurantId,
                request.AvailableDate,
                request.StartTime,
                request.EndTime,
                request.AvailableTables);

            return Ok(new { message = "Availability registered successfully." });
        }


        // Clase simple para recibir datos desde el body
        public class SetAvailabilityApiRequest
        {
            public DateTime AvailableDate { get; set; }
            public TimeSpan StartTime { get; set; }
            public TimeSpan EndTime { get; set; }
            public int AvailableTables { get; set; }
        }
    }
}