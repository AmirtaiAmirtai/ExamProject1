using ExamProject1.Enums;
using ExamProject1.Models;
using System.ComponentModel.DataAnnotations;

namespace ExamProject1.Dto
{
    public class LeadCreateDto
{
    [Required(ErrorMessage = "Field {0} cannot be empty.", AllowEmptyStrings = false)]
    [RegularExpression(@"^\d+$", ErrorMessage = "ContactId must contain only digits.")]
    public required int ContactId { get; set; }

    [Required(ErrorMessage = "Field {0} cannot be empty.", AllowEmptyStrings = false)]
    [RegularExpression(@"^\d+$", ErrorMessage = "Field {0} must contain only digits.")]
    public int SellerId { get; set; }

    [Required(ErrorMessage = "Field {0} cannot be empty.")]
    public required LeadStatus LeadStatus { get; set; }
}
}
