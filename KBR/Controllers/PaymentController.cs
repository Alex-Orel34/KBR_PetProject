using KBR.DbStuff.Models;
using KBR.DbStuff.Repositories;
using KBR.DbStuff.Repositories.Interfaces;
using KBR.Extensions;
using KBR.Models;
using Microsoft.AspNetCore.Mvc;

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
        private readonly ICurrencyRepository _currencyRepository;

        public PaymentController(
            IPaymentRepository paymentRepository,
            ICategoryRepository categoryRepository,
            ICurrencyRepository currencyRepository)
        {
            _paymentRepository = paymentRepository;
            _categoryRepository = categoryRepository;
            _currencyRepository = currencyRepository;
        }

        public async Task<ActionResult> Index()
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var payments = await _paymentRepository.GetUserPaymentsAsync(userId.Value);
            var viewModels = payments.Select(p => new PaymentViewModel
            {
                Id = p.Id,
                PaymentSum = p.PaymentSum,
                Date = p.Date.ToLocalTime(),
                Description = p.Description,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.CategoryName ?? "",
                CategoryColor = p.Category?.Color,
                CategoryIcon = p.Category?.IconUrl,
                CurrencyId = p.CurrencyId,
                CurrencyCode = p.Currency?.CurrencyCode ?? "",
                PaymentType = p.PaymentType,
                PaymentTypeName = p.PaymentType == Enum.PaymentType.Income ? "Доход" : "Расход",
                FormattedAmount = $"{p.PaymentSum:N2} {p.Currency?.CurrencyCode ?? ""}"
            }).ToList();

            return View(viewModels);
        }

        public async Task<IActionResult> Create()
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var model = new CreatePaymentViewModel
            {
                Categories = (await _categoryRepository.GetUserCategoriesAsync(userId.Value))
                    .Select(c => new CategoryViewModel
                    {
                        Id = c.Id,
                        CategoryName = c.CategoryName,
                        Color = c.Color,
                        IconUrl = c.IconUrl
                    }).ToList(),
                Currencies = (await _currencyRepository.GetAllAsync())
                    .Select(c => new CurrencyViewModel
                    {
                        Id = c.Id,
                        CurrencyCode = c.CurrencyCode,
                        CurrencyName = c.CurrencyName
                    }).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePaymentViewModel model)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                var dateUtc = model.Date.Kind == DateTimeKind.Unspecified 
                    ? DateTime.SpecifyKind(model.Date, DateTimeKind.Utc)
                    : model.Date.ToUniversalTime();

                var payment = new Payment
                {
                    PaymentSum = model.PaymentSum,
                    Date = dateUtc,
                    Description = model.Description,
                    CategoryId = model.CategoryId,
                    CurrencyId = model.CurrencyId,
                    PaymentType = model.PaymentType,
                    UserId = userId.Value
                };

                await _paymentRepository.CreateAsync(payment);
                return RedirectToAction(nameof(Index));
            }

            model.Categories = (await _categoryRepository.GetUserCategoriesAsync(userId.Value))
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    CategoryName = c.CategoryName,
                    Color = c.Color,
                    IconUrl = c.IconUrl
                }).ToList();
            model.Currencies = (await _currencyRepository.GetAllAsync())
                .Select(c => new CurrencyViewModel
                {
                    Id = c.Id,
                    CurrencyCode = c.CurrencyCode,
                    CurrencyName = c.CurrencyName
                }).ToList();

            return View(model);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var payment = await _paymentRepository.GetUserPaymentsAsync(userId.Value);
            var specificPayment = payment.FirstOrDefault(p => p.Id == id);

            if (specificPayment == null)
            {
                return NotFound();
            }

            var model = new EditPaymentViewModel
            {
                Id = specificPayment.Id,
                PaymentSum = specificPayment.PaymentSum,
                Date = specificPayment.Date.ToLocalTime(),
                Description = specificPayment.Description,
                CategoryId = specificPayment.CategoryId,
                CurrencyId = specificPayment.CurrencyId,
                PaymentType = specificPayment.PaymentType,
                UserId = userId.Value,
                Categories = (await _categoryRepository.GetUserCategoriesAsync(userId.Value))
                    .Select(c => new CategoryViewModel
                    {
                        Id = c.Id,
                        CategoryName = c.CategoryName,
                        Color = c.Color,
                        IconUrl = c.IconUrl
                    }).ToList(),
                Currencies = (await _currencyRepository.GetAllAsync())
                    .Select(c => new CurrencyViewModel
                    {
                        Id = c.Id,
                        CurrencyCode = c.CurrencyCode,
                        CurrencyName = c.CurrencyName
                    }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditPaymentViewModel model)
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                var dateUtc = model.Date.Kind == DateTimeKind.Unspecified 
                    ? DateTime.SpecifyKind(model.Date, DateTimeKind.Utc)
                    : model.Date.ToUniversalTime();

                var payment = new Payment
                {
                    Id = model.Id,
                    PaymentSum = model.PaymentSum,
                    Date = dateUtc,
                    Description = model.Description,
                    CategoryId = model.CategoryId,
                    CurrencyId = model.CurrencyId,
                    PaymentType = model.PaymentType,
                    UserId = userId.Value
                };

                await _paymentRepository.UpdateAsync(payment, userId.Value);
                return RedirectToAction(nameof(Index));
            }
            model.Categories = (await _categoryRepository.GetUserCategoriesAsync(userId.Value))
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    CategoryName = c.CategoryName,
                    Color = c.Color,
                    IconUrl = c.IconUrl
                }).ToList();
            model.Currencies = (await _currencyRepository.GetAllAsync())
                .Select(c => new CurrencyViewModel
                {
                    Id = c.Id,
                    CurrencyCode = c.CurrencyCode,
                    CurrencyName = c.CurrencyName
                }).ToList();

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

            await _paymentRepository.DeleteAsync(id, userId.Value);
            return RedirectToAction(nameof(Index));
        }


    }
}