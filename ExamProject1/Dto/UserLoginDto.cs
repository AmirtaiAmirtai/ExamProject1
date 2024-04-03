using ExamProject1.Enums;

namespace ExamProject1.Dto
{
    public class UserLoginDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
