using Microsoft.AspNetCore.Mvc;
using PRORC.Application.Interfaces;

namespace PRORC.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class MenusController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenusController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _menuService.GetByIdAsync(id);

            if (result is null)
                return NotFound(new { message = "Menu not found." });

            return Ok(result);
        }

        [HttpGet("restaurant/{restaurantId:int}")]
        public async Task<IActionResult> GetByRestaurant(int restaurantId)
        {
            var result = await _menuService.GetByRestaurantAsync(restaurantId);
            return Ok(result);
        }

        [HttpGet("{menuId:int}/available-items")]
        public async Task<IActionResult> GetAvailableItems(int menuId)
        {
            var result = await _menuService.GetAvailableItemsAsync(menuId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] int restaurantId, [FromQuery] string name)
        {
            try
            {
                var result = await _menuService.CreateAsync(restaurantId, name);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
