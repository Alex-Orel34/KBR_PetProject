using System.ComponentModel.DataAnnotations;
using KBR.Enum;

namespace KBR.Models
{
    /// <summary>
    /// ViewModel для отображения информации о пользователе
    /// Содержит основную информацию о пользователе и статистику
    /// </summary>
    public class UserViewModel
    {
        public Guid Id { get; set; }
        
        [Required]
        [StringLength(100)]
        [Display(Name = "Логин")]
        public string Login { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;
    }
}
