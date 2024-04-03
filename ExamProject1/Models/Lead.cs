
using ExamProject1.Enums;

namespace ExamProject.Models
{
    public class Lead
    {

        public required int Id { get; set; }
        public required int ContactId { get; set; }
        public int? SellerId { get; set; }
        public required LeadStatus LeadStatus { get; set; }
    }
}
