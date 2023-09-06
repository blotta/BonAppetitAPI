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
            return CreatedAtAction(nameof(Restaurant), new { id = newRestaurantId }, null);
        }

        [HttpPut("restaurants/{id:int}")]
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

        [HttpGet("restaurants/{rid:int}/items")]
        public async Task<IActionResult> RestaurantItems([FromRoute] int rid)
        {
            var rest = await _profileService.CurrentUserRestaurant(rid)
                .Include(r => r.MenuItems)
                .SingleOrDefaultAsync();

            if (rest == null)
                return BadRequest();

            var items = _mapper.Map<IEnumerable<MenuItemReadDto>>(rest.MenuItems);

            return Ok(items);
        }

        [HttpGet("restaurants/{rid:int}/items/{itemId:int}")]
        public async Task<IActionResult> RestaurantItem([FromRoute] int rid, [FromRoute] int itemId)
        {
            var item = await _profileService.CurrentUserRestaurant(rid)
                .SelectMany(r => r.MenuItems)
                .SingleOrDefaultAsync(i => i.Id == itemId);

            if (item == null)
                return NotFound();

            var resp = _mapper.Map<MenuItemReadDto>(item);

            return Ok(resp);
        }

        [HttpPost("restaurants/{rid:int}/items")]
        public async Task<IActionResult> RestaurantAddItem([FromRoute] int rid, [FromBody] MenuItemCreateRequestDto request)
        {
            var newItemId = await this._profileService.CreateRestaurantItem(rid, request);
            return CreatedAtAction(nameof(RestaurantItem), new { rid = rid, itemId = newItemId }, null);
        }

        [HttpPut("restaurants/{rid:int}/items/{itemId:int}")]
        public async Task<IActionResult> UpdateRestaurantItem([FromRoute] int rid, [FromRoute] int itemId, [FromBody] MenuItemUpdateRequestDto request)
        {
            try
            {
                await this._profileService.UpdateRestaurantItem(rid, itemId, request);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpDelete("restaurants/{rid:int}/items/{itemId:int}")]
        public async Task<IActionResult> DeleteRestaurantItem([FromRoute] int rid, [FromRoute] int itemId)
        {
            try
            {
                await this._profileService.DeleteRestaurantItem(rid, itemId);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
