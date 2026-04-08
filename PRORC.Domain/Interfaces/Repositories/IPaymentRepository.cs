using PRORC.Domain.Entities.Payments;
using PRORC.Domain.Enums;

namespace PRORC.Domain.Interfaces.Repositories
{
    public interface IPaymentRepository : IBaseRepository<Payment>
    {
        Task<Payment?> GetByOrderIdAsync(int orderId);
        Task<IEnumerable<Payment>> GetByStatusAsync(PaymentStatus status);
        Task<bool> HasAuthorizedPaymentAsync(int orderId);
    }
}