using Microsoft.AspNetCore.Mvc;
using PRORC.Application.DTOs.Payments;
using PRORC.Application.Interfaces;

namespace PRORC.Api.Controllers
{
    [ApiController]
    [Route("api/payments")]

    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }


        // POST que permite crear un pago para una orden
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PaymentRequest request)
        {
            var result = await _paymentService.CreatePaymentAsync(request);
            return Ok(result);
        }


        // GET que permite buscar el pago asociado a una orden
        [HttpGet("by-order/{orderId:int}")]
        public async Task<IActionResult> GetByOrderId(int orderId)
        {
            var result = await _paymentService.GetByOrderIdAsync(orderId);

            if (result == null)
                return NotFound(new { message = "Payment not found for that order." });

            return Ok(result);
        }


        // PATCH que permite autorizar un pago
        [HttpPatch("{paymentId:int}/authorize")]
        public async Task<IActionResult> Authorize(int paymentId)
        {
            var result = await _paymentService.AuthorizePaymentAsync(paymentId);
            return Ok(result);
        }


        // PATCH que permite rechazar un pago
        [HttpPatch("{paymentId:int}/reject")]
        public async Task<IActionResult> Reject(int paymentId)
        {
            await _paymentService.RejectPaymentAsync(paymentId);
            return Ok(new { message = "Payment rejected successfully." });
        }


        // PATCH que permite reembolsar un pago
        [HttpPatch("{paymentId:int}/refund")]
        public async Task<IActionResult> Refund(int paymentId)
        {
            await _paymentService.RefundPaymentAsync(paymentId);
            return Ok(new { message = "Payment refunded successfully." });
        }
    }
}