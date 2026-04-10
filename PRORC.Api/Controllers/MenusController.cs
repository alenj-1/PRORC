using Microsoft.AspNetCore.Mvc;
using PRORC.Application.Interfaces;

namespace PRORC.Api.Controllers
{
    [ApiController]
    [Route("api/menus")]

    public class MenusController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenusController(IMenuService menuService)
        {
            _menuService = menuService;
        }


        // POST que permite crear un nuevo menú
        [HttpPost]
        public async Task<IActionResult> CreateMenu([FromBody] CreateMenuApiRequest request)
        {
            var result = await _menuService.CreateMenuAsync(request.RestaurantId, request.Name);
            return Ok(result);
        }


        // GET que permite obtener todos los menús de un restaurante
        [HttpGet("restaurant/{restaurantId:int}")]
        public async Task<IActionResult> GetByRestaurantId(int restaurantId)
        {
            var result = await _menuService.GetByRestaurantIdAsync(restaurantId);
            return Ok(result);
        }


        // GET que permite obtener el menú activo de un restaurante
        [HttpGet("restaurant/{restaurantId:int}/active")]
        public async Task<IActionResult> GetActiveByRestaurantId(int restaurantId)
        {
            var result = await _menuService.GetActiveByRestaurantIdAsync(restaurantId);

            if (result == null)
                return NotFound(new { message = "Active menu not found." });

            return Ok(result);
        }


        // PUT que permite cambiar el nombre de un menú
        [HttpPut("{menuId:int}/name")]
        public async Task<IActionResult> UpdateMenuName(int menuId, [FromBody] UpdateMenuNameApiRequest request)
        {
            await _menuService.UpdateMenuNameAsync(menuId, request.NewName);
            return Ok(new { message = "Menu name updated successfully." });
        }


        // PATCH que permite activar un menú
        [HttpPatch("{menuId:int}/activate")]
        public async Task<IActionResult> ActivateMenu(int menuId)
        {
            await _menuService.ActivateMenuAsync(menuId);
            return Ok(new { message = "Menu activated successfully." });
        }


        // PATCH que permite desactivar un menú
        [HttpPatch("{menuId:int}/deactivate")]
        public async Task<IActionResult> DeactivateMenu(int menuId)
        {
            await _menuService.DeactivateMenuAsync(menuId);
            return Ok(new { message = "Menu deactivated successfully." });
        }


        // POST que permite agregar un nuevo item a un menú
        [HttpPost("{menuId:int}/items")]
        public async Task<IActionResult> AddMenuItem(int menuId, [FromBody] AddMenuItemApiRequest request)
        {
            var result = await _menuService.AddMenuItemAsync(menuId, request.Name, request.Description, request.Price);
            return Ok(result);
        }


        // GET que permite obtener todos los items de un menú
        [HttpGet("{menuId:int}/items")]
        public async Task<IActionResult> GetItemsByMenuId(int menuId)
        {
            var result = await _menuService.GetItemsByMenuIdAsync(menuId);
            return Ok(result);
        }


        // PUT que permite actualizar el precio de un item del menú
        [HttpPut("items/{menuItemId:int}/price")]
        public async Task<IActionResult> UpdateMenuItemPrice(int menuItemId, [FromBody] UpdateMenuItemPriceApiRequest request)
        {
            await _menuService.UpdateMenuItemPriceAsync(menuItemId, request.NewPrice);
            return Ok(new { message = "Menu item price updated successfully." });
        }


        // PATCH que permite marcar un item del menú como disponible
        [HttpPatch("items/{menuItemId:int}/available")]
        public async Task<IActionResult> MarkAsAvailable(int menuItemId)
        {
            await _menuService.MarkMenuItemAsAvailableAsync(menuItemId);
            return Ok(new { message = "Menu item marked as available." });
        }


        // PATCH que permite marcar un item del menú como no disponible
        [HttpPatch("items/{menuItemId:int}/unavailable")]
        public async Task<IActionResult> MarkAsNotAvailable(int menuItemId)
        {
            await _menuService.MarkMenuItemAsNotAvailableAsync(menuItemId);
            return Ok(new { message = "Menu item marked as not available." });
        }


        // Clases simples para recibir datos desde el body
        public class CreateMenuApiRequest
        {
            public int RestaurantId { get; set; }
            public string Name { get; set; } = string.Empty;
        }

        public class UpdateMenuNameApiRequest
        {
            public string NewName { get; set; } = string.Empty;
        }

        public class AddMenuItemApiRequest
        {
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public decimal Price { get; set; }
        }

        public class UpdateMenuItemPriceApiRequest
        {
            public decimal NewPrice { get; set; }
        }
    }
}