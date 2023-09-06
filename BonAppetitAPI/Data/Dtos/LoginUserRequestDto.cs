using System.ComponentModel.DataAnnotations;

namespace BonAppetitAPI.Data.Dtos
{
    public class LoginUserRequestDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
