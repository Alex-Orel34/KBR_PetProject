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
        private readonly KBRContext _context;
        public PaymentController(IPaymentRepository paymentRepository, ICategoryRepository categoryRepository, KBRContext context)
        {
            _paymentRepository = paymentRepository;
            _categoryRepository = categoryRepository;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            // В реальном приложении здесь будет получение ID текущего пользователя из сессии/токена
            var userId = Guid.NewGuid(); // Заглушка для демонстрации

            var payments = await _paymentRepository.GetUserPaymentsAsync(userId);
            var paymentViewModels = payments.Select(p => new PaymentViewModel
            {
                Id = p.Id,
                PaymentSum = p.PaymentSum,
                Date = p.Date,
                Description = p.Description,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.CategoryName ?? "Неизвестная категория",
                CategoryColor = p.Category?.Color,
                CurrencyId = p.CurrencyId,
                CurrencyCode = p.Currency?.CurrencyCode ?? "RUB",
                UserId = p.UserId,
                FormattedAmount = FormatAmount(p.PaymentSum),
                PaymentType = p.PaymentSum > 0 ? "Доход" : "Расход"
            }).ToList();

            return View(paymentViewModels);
        }
        public async Task<IActionResult> Details(Guid id)
        {
            var userId = Guid.NewGuid(); // Заглушка для демонстрации

            var payment = await _paymentRepository.GetUserPaymentAsync(id, userId);
            if (payment == null)
            {
                return NotFound();
            }

            var viewModel = new PaymentViewModel
            {
                Id = payment.Id,
                PaymentSum = payment.PaymentSum,
                Date = payment.Date,
                Description = payment.Description,
                CategoryId = payment.CategoryId,
                CategoryName = payment.Category?.CategoryName ?? "Неизвестная категория",
                CategoryColor = payment.Category?.Color,
                CurrencyId = payment.CurrencyId,
                CurrencyCode = payment.Currency?.CurrencyCode ?? "RUB",
                UserId = payment.UserId,
                FormattedAmount = FormatAmount(payment.PaymentSum),
                PaymentType = payment.PaymentSum > 0 ? "Доход" : "Расход"
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePaymentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = Guid.NewGuid(); // Заглушка для демонстрации

                // Применяем тип платежа к сумме
                var finalAmount = model.PaymentType == PaymentType.Income
                    ? Math.Abs(model.PaymentSum)
                    : -Math.Abs(model.PaymentSum);

                var payment = new Payment
                {
                    Id = Guid.NewGuid(),
                    PaymentSum = finalAmount,
                    Date = model.Date,
                    Description = model.Description,
                    CategoryId = model.CategoryId,
                    CurrencyId = model.CurrencyId,
                    UserId = userId
                };

                await _paymentRepository.CreateAsync(payment);
                return RedirectToAction(nameof(Index));
            }

            // Перезагружаем списки при ошибке валидации
            var userId = Guid.NewGuid();
            var categories = await _categoryRepository.GetUserCategoriesAsync(userId);
            var currencies = await _context.Currencies.ToListAsync();

            model.Categories = categories.Select(c => new CategoryViewModel
            {
                Id = c.Id,
                CategoryName = c.CategoryName,
                Color = c.Color
            }).ToList();

            model.Currencies = currencies.Select(c => new CurrencyViewModel
            {
                Id = c.Id,
                CurrencyCode = c.CurrencyCode,
                CurrencyName = c.CurrencyName
            }).ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EditPaymentViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var userId = Guid.NewGuid(); // Заглушка для демонстрации

                var payment = await _paymentRepository.GetUserPaymentAsync(id, userId);
                if (payment == null)
                {
                    return NotFound();
                }

                // Применяем тип платежа к сумме
                var finalAmount = model.PaymentType == PaymentType.Income
                    ? Math.Abs(model.PaymentSum)
                    : -Math.Abs(model.PaymentSum);

                payment.PaymentSum = finalAmount;
                payment.Date = model.Date;
                payment.Description = model.Description;
                payment.CategoryId = model.CategoryId;
                payment.CurrencyId = model.CurrencyId;

                await _paymentRepository.UpdateAsync(payment);
                return RedirectToAction(nameof(Index));
            }

            // Перезагружаем списки при ошибке валидации
            var userId = Guid.NewGuid();
            var categories = await _categoryRepository.GetUserCategoriesAsync(userId);
            var currencies = await _context.Currencies.ToListAsync();

            model.Categories = categories.Select(c => new CategoryViewModel
            {
                Id = c.Id,
                CategoryName = c.CategoryName,
                Color = c.Color
            }).ToList();

            model.Currencies = currencies.Select(c => new CurrencyViewModel
            {
                Id = c.Id,
                CurrencyCode = c.CurrencyCode,
                CurrencyName = c.CurrencyName
            }).ToList();

            return View(model);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = Guid.NewGuid(); // Заглушка для демонстрации

            var payment = await _paymentRepository.GetUserPaymentAsync(id, userId);
            if (payment == null)
            {
                return NotFound();
            }

            var viewModel = new PaymentViewModel
            {
                Id = payment.Id,
                PaymentSum = payment.PaymentSum,
                Date = payment.Date,
                Description = payment.Description,
                CategoryId = payment.CategoryId,
                CategoryName = payment.Category?.CategoryName ?? "Неизвестная категория",
                CategoryColor = payment.Category?.Color,
                CurrencyId = payment.CurrencyId,
                CurrencyCode = payment.Currency?.CurrencyCode ?? "RUB",
                UserId = payment.UserId,
                FormattedAmount = FormatAmount(payment.PaymentSum),
                PaymentType = payment.PaymentSum > 0 ? "Доход" : "Расход"
            };

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var userId = Guid.NewGuid(); // Заглушка для демонстрации

            var result = await _paymentRepository.DeleteUserPaymentAsync(id, userId);
            if (!result)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        private string FormatAmount(decimal amount)
        {
            return amount > 0
                ? $"+{amount:C}"
                : $"{amount:C}";
        }
    }
}