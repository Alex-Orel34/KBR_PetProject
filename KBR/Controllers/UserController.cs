using KBR.DbStuff.Models;
using KBR.DbStuff.Repositories;
using KBR.DbStuff.Repositories.Interfaces;
using KBR.Enum;
using KBR.Models;
using Microsoft.AspNetCore.Mvc;

namespace KBR.Controllers
{
    /// <summary>
    /// Контроллер для управления пользователями
    /// Предоставляет CRUD операции для работы с пользователями системы
    /// </summary>
    public class UserController : Controller
    {
        //todo вынести в слой бд
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public async Task<ActionResult> Index()
        {
            var users = await _userRepository.GetByRoleAsync(Role.user);
            return View(users);
        }

        public IActionResult Create()
        {
            return View(new UserViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Login = model.Login,
                    Password = model.Password,
                    Email = model.Email,
                };

                await _userRepository.CreateAsync(user);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _userRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}