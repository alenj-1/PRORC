namespace PRORC.Domain.Entities.Reviews
{
    public class Review
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public int RestaurantId { get; private set; }
        public int ReservationId { get; private set; }
        public int Rating { get; private set; }
        public string Comment { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; }


        private Review() { }

        public static Review Create(int userId, int restaurantId, int reservationId, int rating, string comment, bool hasCompletedService)
        {
            // Validaciones de negocio
            if (userId <= 0)
                throw new ArgumentException("The review must be from a valid customer.");

            if (restaurantId <= 0)
                throw new ArgumentException("The review must be associated with a valid restaurant.");

            if (reservationId <= 0)
                throw new ArgumentException("The review must be associated with a valid reservation.");

            if (!hasCompletedService)
                throw new InvalidOperationException("Only customers with a completed service can leave a review.");

            if (rating < 1 || rating > 5)
                throw new ArgumentException("The rating must be between 1 and 5.");

            if (string.IsNullOrWhiteSpace(comment))
                throw new ArgumentException("The comment cannot be empty.");

            return new Review
            {
                UserId = userId,
                RestaurantId = restaurantId,
                ReservationId = reservationId,
                Rating = rating,
                Comment = comment,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}