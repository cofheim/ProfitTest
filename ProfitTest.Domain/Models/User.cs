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

        public static User Create(string userName, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("Имя пользователя не может быть пустым", nameof(userName));

            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Хэш пароля не может быть пустым", nameof(passwordHash));

            return new User(userName, passwordHash);
        }

        public void UpdateLastLogin()
        {
            LastLoginAt = DateTime.UtcNow;
        }
    }
}
