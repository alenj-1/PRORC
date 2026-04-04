using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRORC.Api.Security;
using PRORC.Application.DTOs.Orders;
using PRORC.Application.Interfaces;
using System.Security.Claims;

namespace PRORC.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

    public class OrdersController(IOrderService orderService) : ControllerBase
    {
        private readonly IOrderService _orderService = orderService;

        // POST que permite crear una orden nueva
        [Authorize(Policy = AuthPolicies.CustomerOnly)]
        [HttpPost]
        public async Task<ActionResult<OrderDto>> Create([FromBody] CreateOrderRequest request)
        {
            var created = await _orderService.CreateAsync(request);
            return Ok(created);
        }


        // GET que permite buscar una orden por su id
        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDto>> GetById(int id)
        {
            var order = await _orderService.GetByIdAsync(id);

            if (order == null)
            {
                return NotFound(new { message = "Order not found." });
            }

            return Ok(order);
        }


        // GET que permite obtener las órdenes de un usuario
        [HttpGet("user/{userId:int}")]
        public async Task<ActionResult<List<OrderDto>>> GetByUser(int userId)
        {
            if (!CanAccessUserData(userId))
            {
                return Forbid();
            }

            var orders = await _orderService.GetByUserAsync(userId);
            return Ok(orders);
        }


        // GET que permite obtener las órdenes que pertenecen a un restaurante
        [Authorize(Policy = AuthPolicies.RestaurantOrSystemAdmin)]
        [HttpGet("restaurant/{restaurantId:int}")]
        public async Task<ActionResult<List<OrderDto>>> GetByRestaurant(int restaurantId)
        {
            var orders = await _orderService.GetByRestaurantAsync(restaurantId);
            return Ok(orders);
        }


        // PATCH que permite confirmar una orden
        [Authorize(Policy = AuthPolicies.RestaurantOrSystemAdmin)]
        [HttpPatch("{orderId:int}/confirm")]
        public async Task<IActionResult> Confirm(int orderId)
        {
            await _orderService.ConfirmAsync(orderId);
            return NoContent();
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
