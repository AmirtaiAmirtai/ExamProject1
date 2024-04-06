using ExamProject1.Enums;
using System.ComponentModel.DataAnnotations;

namespace ExamProject1.Dto
{
    public class ContactCreateDto
{
    [Required(ErrorMessage = "Field {0} cannot be empty.", AllowEmptyStrings = false)]
    public required int MarketologId { get; set; }

    [Required(ErrorMessage = "Field {0} cannot be empty.", AllowEmptyStrings = false)]
    [StringLength(50, ErrorMessage = "Field {0} must not exceed {1} characters.")]
    [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Field {0} must contain only letters.")]
    public required string Name { get; set; }

    [StringLength(50, ErrorMessage = "Field {0} must not exceed {1} characters.")]
    [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Field {0} must contain only letters.")]
    public string? Surname { get; set; }

    [StringLength(50, ErrorMessage = "Field {0} must not exceed {1} characters.")]
    [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Field {0} must contain only letters.")]
    public string? MiddleName { get; set; }

    [Required(ErrorMessage = "Field {0} cannot be empty.", AllowEmptyStrings = false)]
    [StringLength(10, ErrorMessage = "Field {0} must not exceed {1} characters.")]
    [RegularExpression(@"^\d+$", ErrorMessage = "Field {0} must contain only digits.")]
    public required string PhoneNumber { get; set; }

    [EmailAddress(ErrorMessage = "Field {0} has an incorrect format.")]
    [StringLength(200, ErrorMessage = "Field {0} must not exceed {1} characters.")]
    public string? Email { get; set; }
    public required ContactStatus ContactStatus { get; set; }
}
}
