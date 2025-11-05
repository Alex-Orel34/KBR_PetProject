using KBR.DbStuff.Models;

namespace KBR.DbStuff.Repositories
{
    /// <summary>
    /// Предоставляет методы для CRUD операций
    /// </summary>
    public interface IPaymentRepository
    {
        /// <summary>
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <returns>Список платежей пользователя</returns>
        Task<List<Payment>> GetUserPaymentsAsync(Guid userId);

        /// <summary>
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <param name="startDate">Начальная дата периода</param>
        /// <param name="endDate">Конечная дата периода</param>
        /// <returns>Список платежей за указанный период</returns>
        Task<List<Payment>> GetUserPaymentsByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <param name="categoryId">ID категории</param>
        /// <returns>Список платежей в указанной категории</returns>
        Task<List<Payment>> GetUserPaymentsByCategoryAsync(Guid userId, Guid categoryId);

        /// <summary>
        /// </summary>
        /// <param name="payment">Объект платежа для создания</param>
        /// <returns>Созданный платеж с присвоенным ID</returns>
        Task<Payment> CreateAsync(Payment payment);

        /// <summary>
        /// </summary>
        /// <param name="payment">Объект платежа с обновленными данными</param>
        /// <returns>Обновленный платеж</returns>
        Task<Payment> UpdateUserPaymentAsync(Payment payment, Guid userId);

        /// <summary>
        /// </summary>
        /// <param name="id">ID платежа для удаления</param>
        /// <returns>true, если платеж был удален; false, если не найден</returns>
        Task<bool> DeleteUserPaymentAsync(Guid id, Guid userId);
    }
}