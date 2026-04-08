namespace PRORC.Domain.Entities.Menus
{
    public class Menu
    {
        public int Id { get; private set; }
        public int RestaurantId { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public bool IsActive { get; private set; }


        private Menu() { }

        public  static Menu Create(int restaurantId, string name)
        {
            // Validaciones de negocio
            if (restaurantId <= 0)
                throw new ArgumentException("Menu must be associated with a valid restaurant");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Menu name cannot be empty.");

            return new Menu {
                 RestaurantId = restaurantId,
                 Name = name,
                 IsActive = true
            };
        }


        public void Activate()
        {
            if (IsActive)
                throw new InvalidOperationException("Menu is already active.");

            IsActive = true;
        }

        public void Deactivate()
        {
            if (!IsActive)
                throw new InvalidOperationException("Menu is already disabled.");

            IsActive = false;
        }


        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("The menu name cannot be empty.");

            Name = newName;
        }
    }
}