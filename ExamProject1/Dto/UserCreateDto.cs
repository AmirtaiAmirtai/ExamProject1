using ExamProject1.Enums;
using System.ComponentModel.DataAnnotations;

namespace ExamProject1.Dto
{
    public class UserCreateDto
    {
        public string? FullName { get; set; }

        [Required(ErrorMessage = "Field {0} cannot be empty.", AllowEmptyStrings = false)]
        [EmailAddress(ErrorMessage = "Field {0} has an incorrect format.")]
        [StringLength(200, ErrorMessage = "Field {0} must not exceed {1} characters.")]
        public required string Email { get; set; }

        [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = "Field {0} must contain only letters and digits.")]
        [Required(ErrorMessage = "Field {0} cannot be empty.", AllowEmptyStrings = false)]
        [StringLength(10, ErrorMessage = "Field {0} must not exceed {1} characters.")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Field {0} cannot be empty.")]
        public required Role Role { get; set; }
    }
}
