using ExamProject.Models;

namespace ExamProject1.Models
{
    public class Sale
    {

        public required int Id { get; set; }
        public required int LeadId { get; set; }
        public required int? SellerId { get; set; }
        public required DateTime AgreementDate { get; set; }
        public required Lead Lead { get; set; }
        public required User Seller { get; set; }
    }
}
