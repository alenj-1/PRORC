namespace PRORC.Application.DTOs.Orders
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ReservationId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new();
    }
}