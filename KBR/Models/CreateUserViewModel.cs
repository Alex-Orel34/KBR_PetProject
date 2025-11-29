using System.ComponentModel.DataAnnotations;
using KBR.Enum;

namespace KBR.Models
{
    /// <summary>
    /// ViewModel для создания нового пользователя
    /// Содержит все необходимые поля для регистрации пользователя
    /// </summary>
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "Логин обязателен")]
        [StringLength(100, ErrorMessage = "Логин не должен превышать 100 символов")]
        [Display(Name = "Логин")]
        public string Login { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Email обязателен")]
        [StringLength(100, ErrorMessage = "Email не должен превышать 100 символов")]
        [EmailAddress(ErrorMessage = "Некорректный формат email")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Пароль обязателен")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен содержать от 6 до 100 символов")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Подтверждение пароля обязательно")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        public string ConfirmPassword { get; set; } = string.Empty;
        
        /// <summary>
        /// Роль пользователя (по умолчанию user)
        /// Обычно устанавливается автоматически, но может быть изменена администратором
        /// </summary>
        [Display(Name = "Роль")]
        public Role Role { get; set; } = Role.User;
    }
}

