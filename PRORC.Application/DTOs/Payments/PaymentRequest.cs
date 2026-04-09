namespace PRORC.Application.DTOs.Payments
{
    public class PaymentRequest
    {
        public int OrderId { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
    }
}