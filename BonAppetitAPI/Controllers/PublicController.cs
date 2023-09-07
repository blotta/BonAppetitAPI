using AutoMapper;
using AutoMapper.QueryableExtensions;
using BonAppetitAPI.Data;
using BonAppetitAPI.Data.Dtos.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BonAppetitAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class PublicController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PublicController(ApplicationDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        [HttpGet("restaurants")]
        public async Task<IActionResult> GetRestaurants()
        {
            var rests = await _context.Restaurants.ProjectTo<RestaurantReadDto>(_mapper.ConfigurationProvider).ToListAsync();
            return Ok(rests);
        }
    }
}
