namespace PRORC.Domain.Interfaces.Integrations
{
    public interface IPaymentGateway
    {
        Task<string?> AuthorizePaymentAsync(decimal amount, string paymentMethod);
        Task<bool> RefundPaymentAsync(string paymentReference);
    }
}