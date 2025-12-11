using KBR.DbStuff.Models;
using KBR.Enum;
using Microsoft.EntityFrameworkCore;

namespace KBR.DbStuff.Repositories.Interfaces
{
    /// <summary>
    /// Реализует интерфейс ICurrencyRepository и предоставляет методы для работы с базой данных
    /// </summary>
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly KBRContext _context;
        public CurrencyRepository(KBRContext context) => _context = context;
        
        public async Task<Currency> GetCurrencyAsync(Currency currency) => await _context.Currencies
                 .Where(cur => cur.CurrencyCode == currency.CurrencyCode)
                 .FirstOrDefaultAsync();
        
        public async Task<List<Currency>> GetAllAsync() => await _context.Currencies
                 .OrderBy(c => c.CurrencyCode)
                 .ToListAsync();
    }
}
