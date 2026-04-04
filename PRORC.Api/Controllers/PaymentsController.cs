using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRORC.Api.Security;
using PRORC.Application.DTOs.Payments;
using PRORC.Application.Interfaces;

namespace PRORC.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

    public class PaymentsController(IPaymentService paymentService) : ControllerBase
    {
        private readonly IPaymentService _paymentService = paymentService;

        // POST que permite crear un pago a partir de una solicitud
        [Authorize(Policy = AuthPolicies.CustomerOnly)]
        [HttpPost]
        public async Task<ActionResult<PaymentDto>> Create([FromBody] PaymentRequest request)
        {
            var payment = await _paymentService.CreateAsync(request);
            return Ok(payment);
        }

        // GET que permite obtener el pago de una orden
        [HttpGet("order/{orderId:int}")]
        public async Task<ActionResult<PaymentDto>> GetByOrderId(int orderId)
        {
            var payment = await _paymentService.GetByOrderIdAsync(orderId);
            return Ok(payment);
        }

        // PATCH que permite autorizar un pago ya creado
        [Authorize(Policy = AuthPolicies.RestaurantOrSystemAdmin)]
        [HttpPatch("order/{orderId:int}/authorize")]
        public async Task<IActionResult> Authorize(int orderId, [FromQuery] string transactionReference)
        {
            await _paymentService.AuthorizeAsync(orderId, transactionReference);
            return NoContent();
        }
    }
}
