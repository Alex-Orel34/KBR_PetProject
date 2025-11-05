using KBR.DbStuff.Models;

namespace KBR.DbStuff.Repositories.Interfaces
{
    /// <summary>
    /// Реализует интерфейс ICurrencyRepository и предоставляет методы для работы с базой данных
    /// </summary>
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly KBRContext _context;
        public CurrencyRepository(KBRContext context)
        {
            _context = context;
        }

        public Task<Currency> GetCurrencyAsync(Currency currency)
        {
            throw new NotImplementedException();
        }
    }
}
