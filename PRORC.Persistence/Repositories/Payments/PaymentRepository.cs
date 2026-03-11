using Microsoft.EntityFrameworkCore;
using PRORC.Domain.Entities.Payments;
using PRORC.Domain.Enums;
using PRORC.Domain.Interfaces.Repositories;
using PRORC.Persistence.Base;
using PRORC.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Persistence.Repositories.Payments
{
    public class PaymentRepository : BaseRepository<Payment, int>, IPaymentRepository
    {
        public PaymentRepository(PRORCContext context) : base(context)
        {
        }

        public async Task<Payment?> GetPaymentByOrderIdAsync(int orderId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(p => p.OrderId == orderId);
        }

        public async Task<List<Payment>> GetPaymentsByStatusAsync(PaymentStatusEnum status)
        {
            return await _dbSet
                .Where(p => p.Status == status)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<List<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<List<Payment>> GetLatestPaymentsAsync(int count)
        {
            return await _dbSet
                .OrderByDescending(p => p.PaymentDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<bool> ExistsPaymentForOrderAsync(int orderId)
        {
            return await _dbSet
                .AnyAsync(p => p.OrderId == orderId);
        }

        public async Task<bool> HasAuthorizedPaymentAsync(int orderId)
        {
            return await _dbSet
                .AnyAsync(p => p.OrderId == orderId && p.Status == PaymentStatusEnum.Authorized);
        }
    }
}
