using BonAppetitAPI.Data.Dtos;
using BonAppetitAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BonAppetitAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterUserRequestDto requestDto)
        {
            var result = await _authService.RegisterUser(requestDto);

            if (result)
                return Ok("Success");

            return BadRequest();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginUserRequestDto requestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new LoginUserResponseDto { Success = false });

            var result = await _authService.Login(requestDto);

            if (result.Success)
                return Ok(result);

            return Unauthorized(result);
        }

        [HttpGet]
        [Route("testloggedin")]
        [Authorize]
        public async Task<IActionResult> TestLoggedIn()
        {
            var name = User.Identity.Name;
            return Ok($"Logged in as {name}!");
        }
    }
}