using BonAppetitAPI.Data.Dtos;
using BonAppetitAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BonAppetitAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _config;

        public AuthService(
            UserManager<ApplicationUser> userManager
            , RoleManager<IdentityRole> roleManager
            , SignInManager<ApplicationUser> signInManager
            , IConfiguration config)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _config = config;
        }

        public async Task<bool> RegisterUser(RegisterUserRequestDto registerUserDto)
        {
            var identityUser = new ApplicationUser
            {
                UserName = registerUserDto.Email,
                Email = registerUserDto.Email,
                Name = registerUserDto.Name,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(identityUser, registerUserDto.Password);
            if (result.Succeeded)
                await _userManager.SetLockoutEnabledAsync(identityUser, false);

            return result.Succeeded;
        }

        public async Task<LoginUserResponseDto> Login(LoginUserRequestDto loginUserRequestDto)
        {
            var result = await _signInManager.PasswordSignInAsync(loginUserRequestDto.UserName, loginUserRequestDto.Password, false, false);
            if (result.Succeeded)
            {
                var token = await GenerateToken(loginUserRequestDto.UserName);
                return new LoginUserResponseDto
                {
                    Success = true,
                    Token = token
                };
            }

            return new LoginUserResponseDto { Success = false };
        }

        private async Task<string> GenerateToken(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var tokenClaims = await GetUserClaims(user);

            var expirationdate = DateTime.UtcNow.AddSeconds(60 * 60);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));
            var signinCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: _config.GetSection("Jwt:Issuer").Value,
                audience: _config.GetSection("Jwt:Audience").Value,
                claims: tokenClaims,
                notBefore: DateTime.Now,
                expires: expirationdate,
                signingCredentials: signinCredentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return token;
        }
        private async Task<IList<Claim>> GetUserClaims(ApplicationUser user)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.GivenName, user.Name));
            claims.Add(new Claim("name", user.Name));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, DateTime.Now.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()));

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
                claims.Add(new Claim("roles", role));
            }

            return claims;
        }


    }
}
