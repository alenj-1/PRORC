using PRORC.Application.DTOs.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentDto> CreateAsync(PaymentRequest request);
        Task<PaymentDto> GetByOrderIdAsync(int orderId);
        Task AuthorizeAsync(int orderId, string transactionReference);
    }
}
