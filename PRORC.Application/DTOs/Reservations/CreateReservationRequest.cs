namespace PRORC.Application.DTOs.Reservations
{
    public class CreateReservationRequest
    {
        public int UserId { get; set; }
        public int RestaurantId { get; set; }
        public DateTime ReservationDate { get; set; }
        public TimeSpan ReservationTime { get; set; }
        public int NumberOfTables { get; set; }
    }
}