using Microsoft.AspNetCore.Mvc;
using PRORC.Application.DTOs.Payments;
using PRORC.Application.Interfaces;

namespace PRORC.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("order/{orderId:int}")]
        public async Task<IActionResult> GetByOrderId(int orderId)
        {
            var result = await _paymentService.GetByOrderIdAsync(orderId);

            if (result is null)
                return NotFound(new { message = "No payment method found for this order." });

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PaymentRequest request)
        {
            try
            {
                var result = await _paymentService.CreateAsync(request);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{orderId:int}/authorize")]
        public async Task<IActionResult> Authorize(int orderId, [FromQuery] string transactionReference)
        {
            try
            {
                await _paymentService.AuthorizeAsync(orderId, transactionReference);
                return Ok(new { message = "Payment successfully authorized." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
