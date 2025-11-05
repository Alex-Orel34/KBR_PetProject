using KBR.Enum;
using System.ComponentModel.DataAnnotations;

namespace KBR.Models
{
    public class CreatePaymentViewModel
    {
        [Required(ErrorMessage = "Сумма обязательна")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Сумма должна быть больше 0")]
        [Display(Name = "Сумма")]
        public decimal PaymentSum { get; set; }
        
        [Required(ErrorMessage = "Дата обязательна")]
        [Display(Name = "Дата")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Now;
        
        [StringLength(500, ErrorMessage = "Описание не должно превышать 500 символов")]
        [Display(Name = "Описание")]
        public string? Description { get; set; }
        
        [Required(ErrorMessage = "Категория обязательна")]
        [Display(Name = "Категория")]
        public Guid CategoryId { get; set; }
        
        [Required(ErrorMessage = "Валюта обязательна")]
        [Display(Name = "Валюта")]
        public Guid CurrencyId { get; set; }
        
        [Required(ErrorMessage = "Тип платежа обязателен")]
        [Display(Name = "Тип платежа")]
        public PaymentType PaymentType { get; set; } = PaymentType.Expense;
        
        // Списки для выбора
        public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
        public List<CurrencyViewModel> Currencies { get; set; } = new List<CurrencyViewModel>();
    }
}
