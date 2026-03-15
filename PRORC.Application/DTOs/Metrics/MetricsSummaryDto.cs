using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Application.DTOs.Metrics
{
    public class MetricsSummaryDto
    {
        public int TotalUsers { get; set; }
        public int TotalRestaurants { get; set; }
        public int ActiveRestaurants { get; set; }
        public int TotalMenus { get; set; }
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public int TotalReservations { get; set; }
        public int PendingReservations { get; set; }
        public int TotalReviews { get; set; }
        public int AuthorizedPayments { get; set; }
    }
}
