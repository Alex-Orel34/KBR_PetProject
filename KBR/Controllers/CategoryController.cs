using KBR.DbStuff.Models;
using KBR.DbStuff.Repositories;
using KBR.DbStuff.Repositories.Interfaces;
using KBR.Extensions;
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

        public async Task<IActionResult> Index()
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var categories = await _categoryRepository.GetUserCategoriesAsync(userId.Value);
            var viewModels = categories.Select(c => new CategoryViewModel
            {
                Id = c.Id,
                CategoryName = c.CategoryName,
                Color = c.Color,
                IconUrl = c.IconUrl,
                UserId = c.UserId
            }).ToList();

            return View(viewModels);
        }

        public IActionResult Create()
        {
            return View(new CreateCategoryViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryViewModel model)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                var category = new Category
                {
                    Id = Guid.NewGuid(),
                    CategoryName = model.CategoryName,
                    Color = model.Color,
                    IconUrl = model.IconUrl,
                    UserId = userId.Value
                };

                await _categoryRepository.CreateAsync(category);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var category = await _categoryRepository.GetUserCategoryAsync(id, userId.Value);
            if (category == null)
            {
                return NotFound();
            }

            var model = new EditCategoryViewModel
            {
                Id = category.Id,
                CategoryName = category.CategoryName,
                Color = category.Color,
                IconUrl = category.IconUrl,
                UserId = category.UserId
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditCategoryViewModel model)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                var category = await _categoryRepository.GetUserCategoryAsync(model.Id, userId.Value);
                if (category == null)
                {
                    return NotFound();
                }

                category.CategoryName = model.CategoryName;
                category.Color = model.Color;
                category.IconUrl = model.IconUrl;

                await _categoryRepository.UpdateAsync(category);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            await _categoryRepository.DeleteUserCategoryAsync(id, userId.Value);
            return RedirectToAction(nameof(Index));
        }
    }
}