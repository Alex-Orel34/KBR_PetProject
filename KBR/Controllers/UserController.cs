using KBR.DbStuff.Models;
using KBR.DbStuff.Repositories;
using KBR.Models;
using KBR.Enum;
using Microsoft.AspNetCore.Mvc;

namespace KBR.Controllers
{
    /// <summary>
    /// Контроллер для управления пользователями
    /// Предоставляет CRUD операции для работы с пользователями системы
    /// </summary>
    public class UserController : Controller
    {
        private readonly IUserRepository _UserRepository;

        public UserController(IUserRepository userRepository)
        {
            _UserRepository = userRepository;
        }
    }
}