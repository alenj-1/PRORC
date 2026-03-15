using PRORC.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Domain.Entities.Payments
{
    public class Payment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatusEnum Status { get; set; }
        public string TransactionReference { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; }
    }
}
