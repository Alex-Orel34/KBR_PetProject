using System.ComponentModel.DataAnnotations;
using KBR.Enum;

namespace KBR.DbStuff.Models
{
    public class Payment : BaseModel
    {
        [Required]
        public decimal PaymentSum { get; set; }
        
        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        /// <summary>
        /// Тип платежа (доход или расход)
        /// </summary>
        [Required]
        public PaymentType PaymentType { get; set; } = PaymentType.Expense;
        
        [Required]
        public Guid CategoryId { get; set; }
        //todo [NotMapped]
        public virtual Category Category { get; set; } = null!;
        
        [Required]
        public Guid UserId { get; set; }
        //todo [NotMapped]
        public virtual User User { get; set; } = null!;
        
        [Required]
        public Guid CurrencyId { get; set; }
        //todo [NotMapped]
        public virtual Currency Currency { get; set; } = null!;
    }
}
