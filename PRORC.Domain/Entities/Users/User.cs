using PRORC.Domain.Enums;

namespace PRORC.Domain.Entities.Users
{
    public class User
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Password { get; private set; } = string.Empty;
        public UserRole Role { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool IsActive { get; private set; }


        private User() { }

        public static User Create(string name, string email, string password, UserRole role)
        {
            // Validaciones de negocio
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Username cannot be left empty");

            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                throw new ArgumentException("The email address is invalid.");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty");

            if (!Enum.IsDefined(typeof(UserRole), role))
                throw new ArgumentException("Invalid user role.");

            return new User {
                Name = name,
                Email = email,
                Password = password,
                Role = role,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
        }

        public void Deactivate()
        {
            if (!IsActive)
                throw new InvalidOperationException("User has already been deactivated.");

            IsActive = false;
        }

        public void Activate()
        {
            if (IsActive)
                throw new InvalidOperationException("User is already active.");

            IsActive = true;
        }


        public void ChangeRole(UserRole newRole)
        {
            if (!Enum.IsDefined(typeof(UserRole), newRole))
                throw new ArgumentException("The new role is invalid.");

            if (Role == newRole)
                throw new InvalidOperationException("The user already has that role.");

            Role = newRole;
        }
    }
}