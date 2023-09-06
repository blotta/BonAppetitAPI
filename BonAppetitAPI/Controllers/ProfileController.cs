using AutoMapper;
using BonAppetitAPI.Data;
using BonAppetitAPI.Data.Dtos.Profile;
using BonAppetitAPI.Models;
using BonAppetitAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BonAppetitAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ProfileService _profileService;
        private readonly IMapper _mapper;

        public ProfileController(ApplicationDbContext context, ProfileService profileService, IMapper mapper)
        {
            this._context = context;
            this._profileService = profileService;
            this._mapper = mapper;
        }

        public IActionResult Index()
        {
            return Ok();
        }

        [HttpGet("restaurants")]
        public async Task<IActionResult> Restaurants()
        {
            var user = await this._profileService.CurrentUser().Include(u => u.Restaurants).SingleAsync();
            var restaurants = _mapper.Map<IEnumerable<RestaurantReadDto>>(user.Restaurants);
            return Ok(restaurants);
        }

        [HttpGet("restaurants/{id:int}")]
        public async Task<IActionResult> Restaurant([FromRoute] int id)
        {
            var rest = await this._profileService.CurrentUserRestaurants().SingleOrDefaultAsync(r => r.Id == id);
            if (rest == null)
                return NotFound();

            return Ok(_mapper.Map<RestaurantReadDto>(rest));
        }

        [HttpPost("restaurants")]
        public async Task<IActionResult> CreateRestaurant(RestaurantCreateRequestDto requestDto)
        {
            var newRestaurantId = await _profileService.CreateRestaurant(requestDto);
            return CreatedAtAction(nameof(Restaurant), new {id = newRestaurantId}, null);
        }

        [HttpPost("restaurants/{id:int}")]
        public async Task<IActionResult> UpdateRestaurant([FromRoute] int id, [FromBody] RestaurantUpdateRequestDto request)
        {
            try
            {
                await this._profileService.UpdateRestaurant(id, request);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpDelete("restaurants/{id:int}")]
        public async Task<IActionResult> DeleteRestaurant([FromRoute] int id)
        {
            try
            {
                await this._profileService.DeleteRestaurant(id);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
