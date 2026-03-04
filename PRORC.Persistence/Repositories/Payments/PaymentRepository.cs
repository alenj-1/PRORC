using PRORC.Domain.Entities.Payments;
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
    }
}
