using KBR.DbStuff.Models;
using KBR.DbStuff.Repositories;
using KBR.Models;
using Microsoft.AspNetCore.Mvc;

namespace KBR.Controllers
{
    /// <summary>
    /// Контроллер для управления категориями
    /// Предоставляет CRUD операции для работы с категориями
    /// </summary>
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        
    }
}