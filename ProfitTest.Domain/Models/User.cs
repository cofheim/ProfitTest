namespace ProfitTest.Domain.Models
{
    public class User
    {
        public User(string userName, string passwordHash)
        {
            Id = Guid.NewGuid();
            UserName = userName;
            PasswordHash = passwordHash;
            CreatedAt = DateTime.UtcNow;
        }

        public Guid Id { get; }
        public string UserName { get; private set; }
        public string PasswordHash { get; private set; }
        public DateTime CreatedAt { get; }
        public DateTime? LastLoginAt { get; private set; }

        public static (User? User, string Error) Create(string userName, string passwordHash)
        {
            var error = string.Empty;

            if (string.IsNullOrWhiteSpace(userName))
            {
                error = "Имя пользователя не может быть пустым";
                return (null, error);
            }

            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                error = "Хеш пароля не может быть пустым";
                return (null, error);
            }

            var user = new User(userName, passwordHash);
            return (user, error);
        }

        public void UpdateLastLogin()
        {
            LastLoginAt = DateTime.UtcNow;
        }
    }
}
