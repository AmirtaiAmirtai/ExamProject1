using System.ComponentModel.DataAnnotations;

namespace ExamProject1.Dto
{
    public class UserUpdatePasswordDto
    {
        [Required(ErrorMessage = "Поле {0} не может быть пустым.", AllowEmptyStrings = false)]
        public required int Id { get; set; }
        public required string Password { get; set; }
    }
}
