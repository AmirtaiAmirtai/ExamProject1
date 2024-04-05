using ExamProject1.Enums;
using System.ComponentModel.DataAnnotations;

namespace ExamProject1.Dto
{
    public class UserCreateDto
    {

        public string? FullName { get; set; }

        [Required(ErrorMessage = "Поле {0} не может быть пустым.", AllowEmptyStrings = false)]
        [EmailAddress(ErrorMessage = "Поле {0} содержит некорректный формат.")]
        [StringLength(200, ErrorMessage = "Поле {0} не должно превышать {1} символов.")]
        public required string Email { get; set; }

        [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = "Поле {0} должно содержать только буквы и цифры.")]
        [Required(ErrorMessage = "Поле {0} не может быть пустым.", AllowEmptyStrings = false)]
        [StringLength(10, ErrorMessage = "Поле {0} не должно превышать {1} символов.")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Поле {0} не может быть пустым.", AllowEmptyStrings = false)]
        public required Role Role { get; set; }
    }
}
