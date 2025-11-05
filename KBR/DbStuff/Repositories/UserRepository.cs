using KBR.DbStuff.Models;
using KBR.Enum;
using Microsoft.EntityFrameworkCore;

namespace KBR.DbStuff.Repositories.Interfaces
{
    /// <summary>
    /// Реализует интерфейс IUserRepository и предоставляет методы для работы с базой данных
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly KBRContext _context;
        public UserRepository(KBRContext context)
        {
            _context = context;
        }

        public async Task<User?> AuthenticateAsync(string login, string password)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Login == login && u.Password == password);
        }

        public async Task<User> CreateAsync(User user)
        {
            if (user == null) 
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.Role = Role.user;
            user.Id = Guid.NewGuid();

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return false;
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsByLoginAsync(string login)
        {
            return await _context.Users.AnyAsync(u => u.Login == login);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.CreatedCategories)
                .Include(u => u.CreatedPayments)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users
                .Include(u => u.CreatedCategories)
                .Include(u => u.CreatedPayments)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<List<User>> GetByRoleAsync(Role role)
        {
            return await _context.Users
                .Where(u => u.Role == role)
                .Include(u => u.CreatedCategories)
                .Include(u => u.CreatedPayments)
                .ToListAsync();
        }

        public async Task<User> GetUserRoleAsync(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) 
            {
                throw new InvalidOperationException("User not found");
            }
            return user;
        }
    }
}
