using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Domain.Enums
{
    public enum PaymentStatusEnum
    {
        Pending = 1,
        Authorized = 2,
        Failed = 3,
        Refunded = 4
    }
}
