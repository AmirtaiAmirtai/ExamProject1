using ExamProject.Models;
using ExamProject1.Models;

namespace ExamProject1.Dto
{
    public class SaleCreateDto
    {
        public required int LeadId { get; set; }
        public required int? SellerId { get; set; }
        public required DateTime AgreementDate { get; set; }
    }
}
