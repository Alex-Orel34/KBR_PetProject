using System.ComponentModel.DataAnnotations;

namespace KBR.Models
{
    public class CurrencyViewModel
    {
        public Guid Id { get; set; }
        
        [Required]
        [StringLength(3)]
        [Display(Name = "Код валюты")]
        public string CurrencyCode { get; set; } = string.Empty;
        
        [StringLength(50)]
        [Display(Name = "Название валюты")]
        public string? CurrencyName { get; set; }
    }
}

