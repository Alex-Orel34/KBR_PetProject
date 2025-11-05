using KBR.DbStuff;
using KBR.DbStuff.Models;
using KBR.DbStuff.Repositories;
using KBR.Enum;
using KBR.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KBR.Controllers
{
    /// <summary>
    /// Контроллер для управления платежами (доходами/расходами)
    /// Предоставляет CRUD операции для работы с финансовыми транзакциями пользователей
    /// </summary>
    public class PaymentController : Controller
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly KBRContext _context;
        public PaymentController(IPaymentRepository paymentRepository, ICategoryRepository categoryRepository, IUserRepository userRepository, KBRContext context)
        {
            _paymentRepository = paymentRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _context = context;
        }
    }
}