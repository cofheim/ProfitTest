namespace ProfitTest.Persistence.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; } // хэш пароля
        public DateTime CreatedAt { get; set; } // дата создания
        public DateTime? LastLoginAt { get; set; } // дата последнего входа
    }
}
