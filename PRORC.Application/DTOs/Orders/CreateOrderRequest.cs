using PRORC.Application.DTOs.Menus;

namespace PRORC.Application.DTOs.Orders
{
    public class CreateOrderRequest
    {
        public int UserId { get; set; }
        public int RestaurantId { get; set; }
        public int ReservationId { get; set; }
        public List<MenuItemDto> Items { get; set; } = new();
    }
}