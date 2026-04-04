using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRORC.Api.Security;
using PRORC.Application.DTOs.Restaurants;
using PRORC.Application.Interfaces;

namespace PRORC.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class RestaurantsController(IRestaurantService restaurantService) : ControllerBase
    {
        private readonly IRestaurantService _restaurantService = restaurantService;

        // GET que permite obtener un restaurante por id
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<RestaurantDto>> GetById(int id)
        {
            var restaurant = await _restaurantService.GetByIdAsync(id);

            if (restaurant == null)
            {
                return NotFound(new { message = "Restaurant not found." });
            }

            return Ok(restaurant);
        }


        // GET que permite devolver todos los restaurantes que están activo
        [AllowAnonymous]
        [HttpGet("active")]
        public async Task<ActionResult<List<RestaurantDto>>> GetActive()
        {
            var restaurants = await _restaurantService.GetActiveAsync();
            return Ok(restaurants);
        }

        // GET que permite buscar restaurantes
        [AllowAnonymous]
        [HttpGet("search")]
        public async Task<ActionResult<List<RestaurantDto>>> Search([FromQuery] string search)
        {
            var restaurants = await _restaurantService.SearchAsync(search);
            return Ok(restaurants);
        }

        // POST que permite registrar un nuevo restaurante
        [Authorize(Policy = AuthPolicies.RestaurantOrSystemAdmin)]
        [HttpPost]
        public async Task<ActionResult<RestaurantDto>> Create([FromBody] CreateRestaurantRequest request)
        {
            var created = await _restaurantService.CreateAsync(request);
            return Ok(created);
        }
    }
}
