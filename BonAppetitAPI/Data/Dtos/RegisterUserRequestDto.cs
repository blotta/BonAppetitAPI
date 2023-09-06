using System.ComponentModel.DataAnnotations;

namespace BonAppetitAPI.Data.Dtos
{
    public class RegisterUserRequestDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
