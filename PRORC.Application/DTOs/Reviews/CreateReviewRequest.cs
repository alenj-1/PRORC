namespace PRORC.Application.DTOs.Reviews
{
    public class CreateReviewRequest
    {
        public int UserId { get; set; }
        public int RestaurantId { get; set; }
        public int ReservationId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
