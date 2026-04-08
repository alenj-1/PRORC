namespace PRORC.Domain.Entities.Menus
{
    public class MenuItem
    {
        public int Id { get; private set; }
        public int MenuId { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public bool IsAvailable { get; private set; }


        private MenuItem() { }

        public static MenuItem Create(int menuId, string name, string description, decimal price)
        {
            // Validaciones de negocio
            if (menuId <= 0)
                throw new ArgumentException("Item must belong to a valid menu.");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Item name cannot be empty.");

            if (price <= 0)
                throw new ArgumentException("Item price must be greater than zero.");

            return new MenuItem
            {
                MenuId = menuId,
                Name = name,
                Description = description ?? string.Empty,
                Price = price,
                IsAvailable = true
            };
        }


        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice <= 0)
                throw new ArgumentException("Price must be greater than zero.");
        }


        public void MarkAsNotAvailable()
        {
            if (!IsAvailable)
                throw new InvalidOperationException("The item is already marked as not available.");

            IsAvailable = false;
        }

        public void MarkAsAvailable()
        {
            if (IsAvailable)
                throw new InvalidOperationException("The item is already available.");

            IsAvailable = true;
        }
    }
}