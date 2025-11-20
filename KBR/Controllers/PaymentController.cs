using KBR.DbStuff;
using KBR.DbStuff.Models;
using KBR.DbStuff.Repositories;
using KBR.DbStuff.Repositories.Interfaces;
using System.Security.Claims;
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
        private readonly ICategoryRepository _categoryRepository;//todo зачем?
        private readonly IUserRepository _userRepository;//todo зачем?
        private readonly ICurrencyRepository _currencyRepository;//todo зачем?
        private readonly KBRContext _context;//todo зачем?
        public PaymentController(IPaymentRepository paymentRepository, ICategoryRepository categoryRepository, KBRContext context, IUserRepository userRepository, ICurrencyRepository currencyRepository)
        {
            //todo вообще не стоит сувать работу с репозиторием в контроллер, должен быть отдельный бизнес слой
            _paymentRepository = paymentRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _context = context;
            _userRepository = userRepository;
            _currencyRepository = currencyRepository;
        }

        public async Task<ActionResult> Index()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdString == null || !Guid.TryParse(userIdString, out var userId))
            {
                //todo вынести в middleware
                return Unauthorized();
            }

            var payments = await _paymentRepository.GetUserPaymentsAsync(userId);
            return View(payments);
        }

        public IActionResult Create()
        {
            return View(new CreatePaymentViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePaymentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

                //todo вынести в middleware
                if (userIdString == null || !Guid.TryParse(userIdString, out var userId))
                {
                    return Unauthorized();
                }
                var payment = new Payment
                {
                    PaymentSum = model.PaymentSum,
                    Date = model.Date,
                    Description = model.Description,
                    CategoryId = model.CategoryId,
                    CurrencyId = model.CurrencyId,
                    UserId = userId
                };

                await _paymentRepository.CreateAsync(payment);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        //а где [post]?
        public async Task<IActionResult> Edit(Guid id)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //todo вынести в middleware
            if (userIdString == null || !Guid.TryParse(userIdString, out var userId))
            {
                return Unauthorized();
            }

            var payment = await _paymentRepository.GetUserPaymentsAsync(userId);
            var specificPayment = payment.FirstOrDefault(p => p.Id == id);

            if (specificPayment == null)
            {
                return NotFound();
            }

            var model = new EditPaymentViewModel
            {
                Id = specificPayment.Id,
                PaymentSum = specificPayment.PaymentSum,
                Date = specificPayment.Date,
                Description = specificPayment.Description,
                CategoryId = specificPayment.CategoryId,
                CurrencyId = specificPayment.CurrencyId,
                PaymentType = specificPayment.PaymentType
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditPaymentViewModel model)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //todo вынести в middleware
            if (userIdString == null || !Guid.TryParse(userIdString, out var userId))
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                var payment = new Payment
                {
                    Id = model.Id,
                    PaymentSum = model.PaymentSum,
                    Date = model.Date,
                    Description = model.Description,
                    CategoryId = model.CategoryId,
                    CurrencyId = model.CurrencyId,
                    PaymentType = model.PaymentType,
                    UserId = model.UserId
                };

                await _paymentRepository.UpdateAsync(payment, userId);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //todo вынести в middleware
            if (userIdString == null || !Guid.TryParse(userIdString, out var userId))
            {
                return Unauthorized();
            }
            await _paymentRepository.DeleteAsync(id, userId);
            return RedirectToAction(nameof(Index));
        }


    }
}