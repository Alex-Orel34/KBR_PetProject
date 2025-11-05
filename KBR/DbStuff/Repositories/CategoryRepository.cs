using KBR.DbStuff.Models;
using Microsoft.EntityFrameworkCore;

namespace KBR.DbStuff.Repositories
{
    /// <summary>
    /// Реализует интерфейс ICategoryRepository и предоставляет методы для работы с базой данных
    /// </summary>
    public class CategoryRepository : ICategoryRepository
    {
        private readonly KBRContext _context;
        public CategoryRepository(KBRContext context)
        {
            _context = context;
        }
        public async Task<Category> CreateAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteUserCategoryAsync(Guid id, Guid userId)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (category == null) 
            {
                return false;
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Category>> GetUserCategoriesAsync(Guid userId)
        {
            return await _context.Categories
                .Where(c => c.UserId == userId)
                .OrderBy(c => c.CategoryName)
                .ToListAsync();
        }

        public async Task<Category?> GetUserCategoryAsync(Guid id, Guid userId)
        {
            return await _context.Categories
                .Include(c => c.User)
                .Include(c => c.Payments)
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }
    }
}