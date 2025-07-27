namespace ProfitTest.Domain.Models
{
    public class User
    {
        private User(string userName, string passwordHash, Guid? id = null, DateTime? createdAt = null, DateTime? lastLoginAt = null)
        {
            Id = id ?? Guid.NewGuid();
            UserName = userName;
            PasswordHash = passwordHash;
            CreatedAt = createdAt ?? DateTime.UtcNow;
            LastLoginAt = lastLoginAt;
        }

        public Guid Id { get; }
        public string UserName { get; private set; }
        public string PasswordHash { get; private set; } // Хэш пароля
        public DateTime CreatedAt { get; } // Дата создания пользователя
        public DateTime? LastLoginAt { get; private set; } // Дата последнего входа пользователя

        // Фабричный метод для создания пользователя
        public static (User? User, string Error) Create(string userName, string passwordHash)
        {
            return Create(userName, passwordHash, null, null, null);
        }

        // Фабричный метод для создания пользователя с существующими значениями (для восстановления)
        public static (User? User, string Error) Create(string userName, string passwordHash, Guid? id, DateTime? createdAt, DateTime? lastLoginAt)
        {
            var error = string.Empty;

            if (string.IsNullOrWhiteSpace(userName))
            {
                error = "Имя пользователя не может быть пустым";
                return (null, error);
            }

            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                error = "Хэш пароля не может быть пустым";
                return (null, error);
            }

            var user = new User(userName, passwordHash, id, createdAt, lastLoginAt);
            return (user, error);
        }

        // Обновление даты последнего входа
        public void UpdateLastLogin()
        {
            LastLoginAt = DateTime.UtcNow;
        }
    }
}
