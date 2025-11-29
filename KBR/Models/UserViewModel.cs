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
        public string Login { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;

        /// <summary>
        /// Роль пользователя (по умолчанию user)
        /// </summary>
        public Role Role { get; set; } = Role.User;
    }
}
