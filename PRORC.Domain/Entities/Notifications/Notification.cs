namespace PRORC.Domain.Entities.Notifications
{
    public class Notification
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public string Message { get; private set; } = string.Empty;
        public string Type { get; private set; } = string.Empty;
        public bool Read { get; private set; }
        public DateTime SentAt { get; private set; }


        private Notification() { }


        public static Notification Create(int userId, string message, string type)
        {
            // Validaciones de negocio
            if (userId <= 0)
                throw new ArgumentException("The notification must be associated with a valid user.");

            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("The notification message cannot be empty.");

            if (string.IsNullOrWhiteSpace(type))
                throw new ArgumentException("The notification type cannot be empty.");

            return new Notification
            {
                UserId = userId,
                Message = message,
                Type = type,
                Read = false,
                SentAt = DateTime.UtcNow
            };
        }

        public void MarkAsRead()
        {
            if (Read)
                throw new InvalidOperationException("The notification has already been marked as read.");

            Read = true;
        }
    }
}