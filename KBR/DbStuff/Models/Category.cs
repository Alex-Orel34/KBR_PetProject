using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KBR.DbStuff.Models
{
    public class Category : BaseModel
    {
        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? IconUrl { get; set; }
        
        [StringLength(7)]
        public string? Color { get; set; }
        public Guid UserId { get; set; }
        //todo [NotMapped]
        public virtual User User { get; set; } = null!;
        //todo [NotMapped]
        public virtual List<Payment> Payments { get; set; } = new List<Payment>();
    }
}
