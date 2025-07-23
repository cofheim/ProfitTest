using Microsoft.EntityFrameworkCore;
using ProfitTest.Application.Interfaces;
using ProfitTest.Domain.Models;
using ProfitTest.Persistence.Mappings;

namespace ProfitTest.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ProfitTestDbContext _context;

        public UserRepository(ProfitTestDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // получение по id
        public async Task<User?> GetByIdAsync(Guid id)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            return entity?.ToDomain();
        }

        // получение по имени пользователя
        public async Task<User?> GetByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Имя пользователя не может быть пустым", nameof(username));

            var entity = await _context.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == username.ToLower());
            return entity?.ToDomain();
        }

        // создание
        public async Task AddAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var exists = await _context.Users.AnyAsync(x => x.UserName.ToLower() == user.UserName.ToLower());
            if (exists)
                throw new InvalidOperationException($"Пользователь с именем {user.UserName} уже существует");

            var entity = user.ToEntity();
            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        // обновление
        public async Task UpdateAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var updated = await _context.Users
                .Where(x => x.Id == user.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.LastLoginAt, user.LastLoginAt));

            if (updated == 0)
                throw new InvalidOperationException($"Пользователь с ID {user.Id} не найден");
        }
    }
}
