using System.ComponentModel.DataAnnotations;
using KBR.Enum;

namespace KBR.Models
{
    public class PaymentViewModel
    {
        public Guid Id { get; set; }
        
        [Required]
        [Display(Name = "Сумма")]
        public decimal PaymentSum { get; set; }
        
        [Required]
        [Display(Name = "Дата")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.UtcNow;
        
        [StringLength(500)]
        [Display(Name = "Описание")]
        public string? Description { get; set; }
        
        [Required]
        [Display(Name = "Категория")]
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? CategoryColor { get; set; }
        public string? CategoryIcon { get; set; }
        
        [Required]
        [Display(Name = "Валюта")]
        public Guid CurrencyId { get; set; }
        public string CurrencyCode { get; set; } = string.Empty;
        
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Тип платежа (доход или расход)
        /// </summary>
        [Display(Name = "Тип платежа")]
        public PaymentType PaymentType { get; set; } = PaymentType.Expense;
        
        // Дополнительные поля для отображения
        public string FormattedAmount { get; set; } = string.Empty;
        public string PaymentTypeName { get; set; } = string.Empty; // "Доход" или "Расход"
    }
}
