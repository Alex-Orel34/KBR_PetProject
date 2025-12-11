using KBR.DbStuff.Models;

namespace KBR.DbStuff.Repositories.Interfaces
{
    public interface ICurrencyRepository
    {
        /// <summary>
        /// Получить валюту по коду
        /// </summary>
        /// <param name="currency">Код валюты</param>
        /// <returns>Валюта</returns>
        Task<Currency> GetCurrencyAsync(Currency currency);
        
        /// <summary>
        /// Получить все валюты
        /// </summary>
        /// <returns>Список всех валют</returns>
        Task<List<Currency>> GetAllAsync();
    }
}
