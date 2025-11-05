using KBR.DbStuff.Models;

namespace KBR.DbStuff.Repositories.Interfaces
{
    public interface ICurrencyRepository
    {
        /// <summary>
        /// </summary>
        /// <param name="currency">Код валюты</param>
        /// <returns>Валюта</returns>
        Task<Currency> GetCurrencyAsync(Currency currency);
    }
}
