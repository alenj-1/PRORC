namespace PRORC.Domain.Entities.Restaurants
{
    public class RestaurantAvailability
    {
        public int Id { get; private set; }
        public int RestaurantId { get; private set; }
        public DateTime AvailableDate { get; private set; }
        public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }
        public int AvailableTables { get; private set; }


        private RestaurantAvailability() { }

        public static RestaurantAvailability Create(int restaurantId, DateTime availableDate, TimeSpan startTime,
            TimeSpan endTime, int availableTables)
        {
            // Validaciones de negocio
            if (restaurantId <= 0)
                throw new ArgumentException("Restaurant Id is invalid.");

            if (availableDate.Date < DateTime.UtcNow.Date)
                throw new ArgumentException("You cannot enter availability for a past date.");

            if (endTime <= startTime)
                throw new ArgumentException("End time must be later than the start time.");

            if (availableTables < 0)
                throw new ArgumentException("The number of available tables cannot be negative.");

            return new RestaurantAvailability
            {
                RestaurantId = restaurantId,
                AvailableDate = availableDate.Date,
                StartTime = startTime,
                EndTime = endTime,
                AvailableTables = availableTables
            };
        }

        public void ReserveTables(int tablesToReserve)
        {
            if (tablesToReserve <= 0)
                throw new ArgumentException("You must reserve at least one table.");

            if (AvailableTables < tablesToReserve)
                throw new InvalidOperationException("There are no tables available at this time.");

            AvailableTables -= tablesToReserve;
        }

        public void FreeTables(int tablesToRelease)
        {
            if (tablesToRelease <= 0)
                throw new ArgumentException("You must release at least one table.");

            AvailableTables += tablesToRelease;
        }
    }
}