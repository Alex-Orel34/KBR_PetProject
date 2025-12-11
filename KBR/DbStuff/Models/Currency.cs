using System.ComponentModel.DataAnnotations;

namespace KBR.DbStuff.Models
{
    public class Currency : BaseModel
    {
        [Required]
        [StringLength(3)]
        public string CurrencyCode { get; set; } = "RUB";
        
        [StringLength(50)]
        public string? CurrencyName { get; set; }
        //todo [NotMapped]
        public virtual List<Payment> Payments { get; set; } = new List<Payment>();
    }
}
