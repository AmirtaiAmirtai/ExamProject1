using ExamProject1.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamProject1.Models
{
    public class Contact
    {
        public required int Id { get; set; }
        public required int MarketologId { get; set; }
        public required string Name { get; set; }
        public string? Surname { get; set; }
        public string? MiddleName { get; set; }
        public required string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public required ContactStatus ContactStatus { get; set; }
        public required DateTime LastUpdate { get; set; }
        public User Marketolog { get; set; }  
    }
}
