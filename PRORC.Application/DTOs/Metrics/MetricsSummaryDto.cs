namespace PRORC.Application.DTOs.Metrics
{
    public class MetricsSummaryDto
    {
        public int RestaurantId { get; set; }
        public int TotalReservations { get; set; }
        public int CompletedReservations { get; set; }
        public int TotalOrders { get; set; }
        public int CompletedOrders { get; set; }
        public int TotalReviews { get; set; }
        public double AverageRating { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}