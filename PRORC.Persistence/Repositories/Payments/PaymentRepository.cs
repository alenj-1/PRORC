using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PRORC.Domain.Entities.Payments;
using PRORC.Domain.Enums;
using PRORC.Domain.Interfaces.Repositories;
using PRORC.Persistence.Base;
using PRORC.Persistence.Context;

namespace PRORC.Persistence.Repositories.Payments
{
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(PRORCContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory) { }

        public async Task<Payment?> GetByOrderIdAsync(int orderId)
        {
            try
            {
                return await _context.Payments
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.OrderId == orderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payment by OrderId {OrderId}.", orderId);
                throw;
            }
        }

        public async Task<IEnumerable<Payment>> GetByStatusAsync(PaymentStatus status)
        {
            try
            {
                return await _context.Payments
                    .AsNoTracking()
                    .Where(p => p.Status == status)
                    .OrderByDescending(p => p.PaymentDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payments by Status {Status}.", status);
                throw;
            }
        }

        public async Task<bool> HasAuthorizedPaymentAsync(int orderId)
        {
            try
            {
                return await _context.Payments
                    .AnyAsync(p => p.OrderId == orderId && p.Status == PaymentStatus.Authorized);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking authorized payment for OrderId {OrderId}.", orderId);
                throw;
            }
        }
    }
}