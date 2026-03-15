using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Domain.Interfaces.Logging
{
    public interface IAuditLogger
    {
        Task LogAsync(string action, string details);
    }
}
