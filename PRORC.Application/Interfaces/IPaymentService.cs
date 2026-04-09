using PRORC.Application.DTOs.Payments;

namespace PRORC.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentDto> CreatePaymentAsync(PaymentRequest request);
        Task<PaymentDto?> GetByOrderIdAsync(int orderId);
        Task<PaymentDto> AuthorizePaymentAsync(int paymentId);
        Task RejectPaymentAsync(int paymentId);
        Task RefundPaymentAsync(int paymentId);
    }
}