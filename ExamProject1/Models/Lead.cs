
using ExamProject1.Enums;
using ExamProject1.Models;
using System.ComponentModel.DataAnnotations;

namespace ExamProject.Models
{
    public class Lead
    {

        public int Id { get; set; }
        public required int ContactId { get; set; }
        public int? SellerId { get; set; }
        public required LeadStatus LeadStatus { get; set; }
        public Contact? Contact { get; set; }
        public User? Seller { get; set; }
        public List<Sale> Sales { get; set; }
    }
}
