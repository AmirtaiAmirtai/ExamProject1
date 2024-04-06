using ExamProject.Models;
using ExamProject1.Models;
using System.ComponentModel.DataAnnotations;

namespace ExamProject1.Dto
{
    public class SaleCreateDto
    {
        [Required(ErrorMessage = "Field {0} cannot be empty.")]
        public required int LeadId { get; set; }

        [Required(ErrorMessage = "Field {0} cannot be empty.")]
        public required int SellerId { get; set; }

        [Required(ErrorMessage = "Field {0} cannot be empty.")]
        [DataType(DataType.DateTime, ErrorMessage = "Field {0} must be a valid date and time.")]
        public required DateTime AgreementDate { get; set; }
    }
}
