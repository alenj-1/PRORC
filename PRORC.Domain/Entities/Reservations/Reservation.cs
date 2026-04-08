using PRORC.Domain.Enums;

namespace PRORC.Domain.Entities.Reservations
{
    public class Reservation
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public int RestaurantId { get; private set; }
        public DateTime ReservationDate { get; private set; }
        public TimeSpan ReservationTime { get; private set; }
        public int NumberOfTables { get; private set; }
        public ReservationStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }


        private Reservation() { }

        public static Reservation Create(int userId, int restaurantId, DateTime reservationDate, TimeSpan reservationTime, int numberOfTables)
        {
            // Validaciones de negocio
            if (userId <= 0)
                throw new ArgumentException("The reservation must be made by a valid customer.");

            if (restaurantId <= 0)
                throw new ArgumentException("The reservation must be linked to a valid restaurant.");

            if (reservationDate.Date < DateTime.UtcNow.Date)
                throw new ArgumentException("You cannot make a reservation for a past date.");

            if (numberOfTables <= 0)
                throw new ArgumentException("At least one table must be reserved.");

            return new Reservation {
                UserId = userId,
                RestaurantId = restaurantId,
                ReservationDate = reservationDate.Date,
                ReservationTime = reservationTime,
                NumberOfTables = numberOfTables,
                Status = ReservationStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };
        }


        public void Confirm()
        {
            if (Status != ReservationStatus.Pending)
                throw new InvalidOperationException("Only pending reservations can be confirmed.");

            Status = ReservationStatus.Confirmed;
        }


        public void Complete()
        {
            if (Status != ReservationStatus.Confirmed)
                throw new InvalidOperationException("Only confirmed reservations can be completed.");

            Status = ReservationStatus.Completed;
        }


        public void Cancel()
        {
            if (Status == ReservationStatus.Completed)
                throw new InvalidOperationException("A completed reservation cannot be cancelled.");

            if (Status == ReservationStatus.Cancelled)
                throw new InvalidOperationException("The reservation has already been cancelled."); 

            Status = ReservationStatus.Cancelled;
        }
    }
}