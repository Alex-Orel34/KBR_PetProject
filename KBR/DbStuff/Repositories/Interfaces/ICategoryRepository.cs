using KBR.DbStuff.Models;

namespace KBR.DbStuff.Repositories
{
    public interface ICategoryRepository
    {

        /// <summary>
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <returns>Список категорий пользователя</returns>
        Task<List<Category>> GetUserCategoriesAsync(Guid userId);

        /// <summary>
        /// </summary>
        /// <param name="id">ID платежа</param>
        /// <param name="userId">ID пользователя</param>
        /// <returns>Платеж пользователя</returns>
        Task<Category?> GetUserCategoryAsync(Guid id, Guid userId);

        /// <summary>
        /// </summary>
        /// <param name="category">Категория</param>
        /// <returns>Создает категорию</returns>
        Task<Category> CreateAsync(Category category);

        /// <summary>
        /// </summary>
        /// <param name="category">Категория</param>
        /// <returns>Обновляет категорию</returns>
        Task<Category> UpdateAsync(Category category);

        /// <summary>
        /// </summary>
        /// <param name="id">ID платежа</param>
        /// <param name="userId">ID пользователя</param>
        /// <returns>Удаляет категорию пользователя</returns>
        Task<bool> DeleteUserCategoryAsync(Guid id, Guid userId);
    }
}
