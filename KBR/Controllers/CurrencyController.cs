using KBR.DbStuff;
using KBR.DbStuff.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KBR.Controllers
{
    [Authorize]
    public class CurrencyController : Controller
    {
        private readonly KBRContext _context;
        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyController(KBRContext context, ICurrencyRepository currencyRepository)
        {
            _context = context;
            _currencyRepository = currencyRepository;
        }

        /// <summary>
        /// Получить список всех валют
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var currencies = await _currencyRepository.GetAllAsync();
            return Json(currencies.Select(c => new { c.Id, c.CurrencyCode, c.CurrencyName }));
        }
    }
}

