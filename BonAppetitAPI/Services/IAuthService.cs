using BonAppetitAPI.Data.Dtos;
using BonAppetitAPI.Models;

namespace BonAppetitAPI.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterUser(RegisterUserRequestDto registerUserDto);
        Task<LoginUserResponseDto> Login(LoginUserRequestDto loginUserRequestDto);
    }
}
