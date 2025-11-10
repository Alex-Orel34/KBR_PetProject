using KBR.DbStuff.Models;
using Microsoft.EntityFrameworkCore;

namespace KBR.DbStuff.Repositories
{
    /// <summary>
    /// Реализует интерфейс IPaymentRepository и предоставляет методы для работы с базой данных
    /// </summary>
    public class PaymentRepository : IPaymentRepository
    {
        private readonly KBRContext _context;
        public PaymentRepository(KBRContext context)
        {
            _context = context;
        }
        public async Task<Payment> CreateAsync(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;

        }

        public async Task<bool> DeleteAsync(Guid id, Guid userId)
        {
            var payment = await _context.Payments
                .Where(p => p.UserId == userId)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (payment == null)
            {
                return false;
            }
            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Payment>> GetUserPaymentsAsync(Guid userId)
        {
            return await _context.Payments
                .Where(p => p.UserId == userId)
                .Include(p => p.Category)
                .Include(p => p.Currency)
                .OrderByDescending(p => p.Date)
                .ToListAsync();
        }

        public async Task<List<Payment>> GetUserPaymentsByCategoryAsync(Guid userId, Guid categoryId)
        {
            return await _context.Payments
                .Where(p => p.UserId == userId && p.CategoryId == categoryId)
                .Include(p => p.Currency)
                .OrderByDescending(p => p.Date)
                .ToListAsync();
        }

        public async Task<List<Payment>> GetUserPaymentsByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate)
        {
            return await _context.Payments
                .Where(p => p.UserId == userId && p.Date >= startDate && p.Date <= endDate)
                .Include(p => p.Category)
                .Include(p => p.Currency)
                .OrderByDescending(p => p.Date)
                .ToListAsync();
        }

        public async Task<Payment> UpdateAsync(Payment payment, Guid userId)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
            return payment;
        }
    }
}