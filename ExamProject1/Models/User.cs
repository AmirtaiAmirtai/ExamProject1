using ExamProject1.Enums;
using System.ComponentModel.DataAnnotations;

namespace ExamProject1.Models
{
    public class User

    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required Role Role { get; set; }
        public DateTime? BanDate { get; set; }
    }
}
