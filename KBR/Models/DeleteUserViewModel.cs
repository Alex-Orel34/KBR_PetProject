using System.ComponentModel.DataAnnotations;
using KBR.Enum;

namespace KBR.Models
{
    /// <summary>
    /// ViewModel для создания нового пользователя
    /// Содержит все необходимые поля для регистрации пользователя
    /// </summary>
    public class DeleteUserViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Логин обязателен")]
        [StringLength(100, ErrorMessage = "Логин не должен превышать 100 символов")]
        [Display(Name = "Логин")]
        public string Login { get; set; } = string.Empty;
    }
}

