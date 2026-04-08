namespace PRORC.Domain.Entities.Orders
{
    public class OrderItem
    {
        public int Id { get; private set; }
        public int OrderId { get; private set; }
        public int MenuItemId { get; private set; }
        public string ItemName { get; private set; } = string.Empty;
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal Subtotal {  get; private set; }


        private OrderItem() { }

        public static OrderItem Create(int orderId, int menuItemId, string itemName, int quantity, decimal unitPrice)
        {
            // Validaciones de negocio
            if (orderId <= 0)
                throw new ArgumentException("The item must be part of a valid order.");

            if (menuItemId <= 0)
                throw new ArgumentException("The item must reference a valid menu item.");

            if (string.IsNullOrWhiteSpace(itemName))
                throw new ArgumentException("The item name cannot be empty.");

            if (quantity <= 0)
                throw new ArgumentException("The amount must be greater than zero.");

            if (unitPrice <= 0)
                throw new ArgumentException("The unit price must be greater than zero.");

            return new OrderItem
            {
                OrderId = orderId,
                MenuItemId = menuItemId,
                ItemName = itemName,
                Quantity = quantity,
                UnitPrice = unitPrice,
                Subtotal = quantity * unitPrice
            };
        }
    }
}