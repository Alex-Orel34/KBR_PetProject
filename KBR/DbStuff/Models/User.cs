using System.ComponentModel.DataAnnotations;
using KBR.Enum;

namespace KBR.DbStuff.Models
{
    public class User : BaseModel
    {
        [Required]
        [StringLength(100)]
        public string Login { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string Password { get; set; } = string.Empty;
        
        /// <summary>
        /// Роль пользователя в системе
        /// По умолчанию устанавливается роль "user"
        /// </summary>
        public Role Role { get; set; } = Role.user;
        
        public virtual List<Category> CreatedCategories { get; set; } = new List<Category>();
        public virtual List<Payment> CreatedPayments { get; set; } = new List<Payment>();
    }
}
