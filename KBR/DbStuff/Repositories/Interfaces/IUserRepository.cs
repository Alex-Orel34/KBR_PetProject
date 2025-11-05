using KBR.DbStuff.Models;
using KBR.Enum;
using Microsoft.EntityFrameworkCore;

namespace KBR.DbStuff.Repositories
{
    /// <summary>
    /// Предоставляет методы для CRUD операций и аутентификации
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// </summary>
        /// <param name="id">ID пользователя</param>
        /// <returns>Авторизован или null, если нет</returns>
        Task<User?> AuthenticateAsync(string login, string password);

        /// <summary>
        /// </summary>
        /// <param name="id">ID пользователя</param>
        /// <returns>Пользователь</returns>
        Task<User> CreateAsync(User user);

        /// <summary>
        /// </summary>
        /// <param name="id">ID пользователя</param>
        /// <returns>Удаление</returns>
        Task<bool> DeleteAsync(Guid id);

        Task<bool> ExistsByLoginAsync(string login);

        Task<User?> GetByEmailAsync(string email);

        /// <summary>
        /// </summary>
        /// <param name="id">ID пользователя</param>
        /// <returns>Пользователь</returns>
        Task<User?> GetByIdAsync(Guid id);

        Task<List<User>> GetByRoleAsync(Role role);

        /// <summary>
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <returns>Пользователь</returns>
        Task<User> GetUserRoleAsync(Guid userId);
    }
}
