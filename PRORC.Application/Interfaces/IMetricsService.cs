using PRORC.Application.DTOs.Metrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Application.Interfaces
{
    public interface IMetricsService
    {
        Task<MetricsSummaryDto> GetSummaryDTOAsync();
    }
}
