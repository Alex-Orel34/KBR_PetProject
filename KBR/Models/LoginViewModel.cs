using System.ComponentModel.DataAnnotations;

namespace KBR.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Логин обязателен")]
        [Display(Name = "Логин")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "Пароль обязателен")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }
    }
}

