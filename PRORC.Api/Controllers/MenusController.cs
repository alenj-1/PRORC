using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRORC.Api.Security;
using PRORC.Application.Interfaces;
using PRORC.Application.DTOs.Menus;

namespace PRORC.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class MenusController(IMenuService menuService) : ControllerBase
    {
        private readonly IMenuService _menuService = menuService;

        // GET que permite obtener un menú por su Id
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<MenuDto>> GetById(int id)
        {
            var menu = await _menuService.GetByIdAsync(id);

            if (menu == null)
            {
                return NotFound(new { message = "Menu not found." });
            }
            return Ok(menu);
        }


        // GET que permite obtener los menús de un restaurante
        [AllowAnonymous]
        [HttpGet("restaurant/{restaurantId:int}")]
        public async Task<ActionResult<List<MenuDto>>> GetByRestaurant(int restaurantId)
        {
            var menus = await _menuService.GetByRestaurantAsync(restaurantId);
            return Ok(menus);
        }


        // POST que permite crear un nuevo menú para un restaurante
        [Authorize(Policy = AuthPolicies.RestaurantOrSystemAdmin)]
        [HttpPost("restaurant/{restaurantId:int}")]
        public async Task<ActionResult<MenuDto>> Create(int restaurantId, [FromQuery] string name)
        {
            var created = await _menuService.CreateAsync(restaurantId, name);
            return Ok(created);
        }


        // GET que permite obtener los items disponibles para un menú específico
        [AllowAnonymous]
        [HttpGet("{menuId:int}/items/available")]
        public async Task<ActionResult<List<MenuItemDto>>> GetAvailableItems(int menuId)
        {
            var items = await _menuService.GetAvailableItemsAsync(menuId);
            return Ok(items);
        }
    }
}