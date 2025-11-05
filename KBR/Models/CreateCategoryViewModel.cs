using System.ComponentModel.DataAnnotations;

namespace KBR.Models
{
    public class CreateCategoryViewModel
    {
        [Required(ErrorMessage = "Название категории обязательно")]
        [Display(Name = "Название категории")]
        public string CategoryName { get; set; } = string.Empty;

        [Display(Name = "URL иконки")]
        public string? IconUrl { get; set; }
        
        [StringLength(7, ErrorMessage = "Цвет должен быть в формате #RRGGBB")]
        [RegularExpression(@"^#[0-9A-Fa-f]{6}$", ErrorMessage = "Цвет должен быть в формате #RRGGBB")]
        [Display(Name = "Цвет")]
        public string? Color { get; set; }
    }
}

