namespace PRORC.Application.DTOs.Menus
{
    public class MenuItemDto
    {
        public int Id { get; set; }
        public int MenuId { get; set; }
        public int MenuItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }

        public string ItemName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal {  get; set; }
    }
}