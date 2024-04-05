using ExamProject1.Enums;
using System.ComponentModel.DataAnnotations;

namespace ExamProject1.Dto
{
    public class ContactChangeDto
    {
        [Required(ErrorMessage = "Поле {0} не может быть пустым.", AllowEmptyStrings = false)]
        public required int MarketologId { get; set; }
        [Required(ErrorMessage = "Поле {0} не может быть пустым.", AllowEmptyStrings = false)]
        [StringLength(50, ErrorMessage = "Поле {0} не должно превышать {1} символов.")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Поле {0} должно содержать только буквы.")]
        public required string Name { get; set; }

        [StringLength(50, ErrorMessage = "Поле {0} не должно превышать {1} символов.")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Поле {0} должно содержать только буквы.")]
        public string? Surname { get; set; }

        [StringLength(50, ErrorMessage = "Поле {0} не должно превышать {1} символов.")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Поле {0} должно содержать только буквы.")]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Поле {0} не может быть пустым.", AllowEmptyStrings = false)]
        [StringLength(10, ErrorMessage = "Поле {0} не должно превышать {1} символов.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Поле {0} должно содержать только цифры.")]
        public required string PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Поле {0} содержит некорректный формат.")]
        [StringLength(200, ErrorMessage = "Поле {0} не должно превышать {1} символов.")]
        public string? Email { get; set; }
        public required ContactStatus ContactStatus { get; set; }
    }
}
