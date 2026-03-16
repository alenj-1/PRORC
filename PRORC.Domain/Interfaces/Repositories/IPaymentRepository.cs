using PRORC.Domain.Entities.Payments;
using PRORC.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Domain.Interfaces.Repositories
{
    public interface IPaymentRepository : IBaseRepository<Payment, int>
    {
        Task<Payment?> GetPaymentByOrderIdAsync(int orderId);
        Task<List<Payment>> GetPaymentsByStatusAsync(PaymentStatusEnum status);
        Task<List<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<Payment>> GetLatestPaymentsAsync(int count);
        Task<bool> ExistsPaymentForOrderAsync(int orderId);
        Task<bool> HasAuthorizedPaymentAsync(int orderId);
    }
}
