using PRORC.Domain.Entities.Reviews;

namespace PRORC.Domain.Entities.Restaurants
{
    public class Restaurant
    {
        public int Id { get; private set; }
        public int OwnerId { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string CuisineType { get; private set; } = string.Empty;
        public string Address { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public double Rating { get; private set; }
        public bool IsActive { get; private set; }


        private Restaurant() { }

        public static Restaurant Create(int ownerId, string name, string cuisineType, string address, string description)
        {
            // Validaciones de negocio
            if (ownerId <= 0)
                throw new ArgumentException("The restaurant must have a valid owner");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Restaurant name cannot be empty");

            if (string.IsNullOrWhiteSpace(cuisineType))
                throw new ArgumentException("Cuisine type cannot be empty");

            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Restaurant address cannot be empty");

            return new Restaurant
            {
                OwnerId = ownerId,
                Name = name,
                CuisineType = cuisineType,
                Address = address,
                Description = description,
                Rating = 0.0,
                IsActive = true
            };
        }


        public void UpdateInfo(string name, string cuisineType, string address, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.");

            Name = name;
            CuisineType = cuisineType;
            Address = address;
            Description = description ?? "";
        }


        public void SetCalculatedRating(double calculateRating)
        {
            if (calculateRating < 0 || calculateRating > 5)
                throw new ArgumentException("Calculated rating must be between 0 and 5.");

            Rating = Math.Round(calculateRating, 2);
        }


        public void Deactivate()
        {
            if (!IsActive)
                throw new InvalidOperationException("The estaurant has already been deactivated.");

            IsActive = false;
        }


        public void Activate()
        {
            if (IsActive)
                throw new InvalidOperationException("The restaurant is already active.");

            IsActive = true;
        }
    }
}