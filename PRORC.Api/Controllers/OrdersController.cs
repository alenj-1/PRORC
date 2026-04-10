using Microsoft.AspNetCore.Mvc;
using PRORC.Application.DTOs.Orders;
using PRORC.Application.Interfaces;

namespace PRORC.Api.Controllers
{
    [ApiController]
    [Route("api/orders")]

    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        // POST que permite crear una nueva orden
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
        {
            var result = await _orderService.CreateOrderAsync(request);
            return Ok(result);
        }


        // GET que permite obtener una orden por Id
        [HttpGet("{orderId:int}")]
        public async Task<IActionResult> GetById(int orderId)
        {
            var result = await _orderService.GetByIdAsync(orderId);

            if (result == null)
                return NotFound(new { message = "Order not found." });

            return Ok(result);
        }


        // GET que permite obtener la orden asociada a una reserva
        [HttpGet("reservation/{reservationId:int}")]
        public async Task<IActionResult> GetByReservationId(int reservationId)
        {
            var result = await _orderService.GetByReservationIdAsync(reservationId);

            if (result == null)
                return NotFound(new { message = "Order not found for that reservation." });

            return Ok(result);
        }


        // GET que permite obtener todas las órdenes de un usuario
        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var result = await _orderService.GetByUserIdAsync(userId);
            return Ok(result);
        }


        // PATCH que permite confirmar una orden
        [HttpPatch("{orderId:int}/confirm")]
        public async Task<IActionResult> Confirm(int orderId)
        {
            await _orderService.ConfirmOrderAsync(orderId);
            return Ok(new { message = "Order confirmed successfully." });
        }


        // PATCH que permite completar una orden
        [HttpPatch("{orderId:int}/complete")]
        public async Task<IActionResult> Complete(int orderId)
        {
            await _orderService.CompleteOrderAsync(orderId);
            return Ok(new { message = "Order completed successfully." });
        }


        // PATCH que permite cancelar una orden
        [HttpPatch("{orderId:int}/cancel")]
        public async Task<IActionResult> Cancel(int orderId)
        {
            await _orderService.CancelOrderAsync(orderId);
            return Ok(new { message = "Order cancelled successfully." });
        }
    }
}