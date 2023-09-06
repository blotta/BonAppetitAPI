using AutoMapper;
using BonAppetitAPI.Data;
using BonAppetitAPI.Data.Dtos.Profile;
using BonAppetitAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BonAppetitAPI.Services
{
    public class ProfileService
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IMapper _mapper;

        public ProfileService(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContext, IMapper mapper)
        {
            _context = applicationDbContext;
            _userManager = userManager;
            _httpContextAccessor = httpContext;
            _mapper = mapper;
        }

        private ClaimsIdentity GetPrincipal()
        {
            return (ClaimsIdentity)_httpContextAccessor.HttpContext!.User.Identity!;
        }

        private string GetUserId()
        {
            return GetPrincipal().FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value;
        }

        public IQueryable<ApplicationUser> CurrentUser()
        {
            var userId = GetUserId();
            return _context.Users.Where(u => u.Id == userId).AsQueryable();
        }

        public async Task<int> CreateRestaurant(RestaurantCreateRequestDto dto)
        {
            var user = await CurrentUser().Include(u => u.Restaurants).SingleAsync();
            var newRest = _mapper.Map<Restaurant>(dto);
            user.Restaurants.Add(newRest);
            await _context.SaveChangesAsync();
            await _context.Entry(newRest).ReloadAsync();
            return newRest.Id;
        }

        public IQueryable<Restaurant> CurrentUserRestaurants()
        {
            return CurrentUser().Include(u => u.Restaurants).SelectMany(u => u.Restaurants).AsQueryable();
        }

        public IQueryable<Restaurant> CurrentUserRestaurant(int rid)
        {
            var userId = GetUserId();
            return _context.Restaurants.Where(r => r.Owner.Id == userId && r.Id == rid);
            // return CurrentUser().Include(u => u.Restaurants).SelectMany(u => u.Restaurants).Single(r => r.Id == rid).AsQueryable();
        }

        public async Task UpdateRestaurant(int id, RestaurantUpdateRequestDto dto)
        {
            var rest = await this.CurrentUserRestaurants().SingleOrDefaultAsync(r => r.Id == id);
            if (rest == null)
                throw new Exception("Restaurante Inválido");

            _mapper.Map(dto, rest);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRestaurant(int id)
        {
            var rest = await this.CurrentUserRestaurants().SingleOrDefaultAsync(r => r.Id == id);
            if (rest == null)
                throw new Exception("Restaurante Inválido");

            _context.Restaurants.Remove(rest);
            await _context.SaveChangesAsync();
        }

        public async Task<int> CreateRestaurantItem(int restaurantId, MenuItemCreateRequestDto dto)
        {
            var rest = await CurrentUserRestaurants().Include(u => u.MenuItems).SingleAsync(r => r.Id == restaurantId);
            var newItem = _mapper.Map<MenuItem>(dto);
            rest.MenuItems.Add(newItem);
            await _context.SaveChangesAsync();
            await _context.Entry(newItem).ReloadAsync();
            return newItem.Id;
        }

        public async Task UpdateRestaurantItem(int restaurantId, int itemId, MenuItemUpdateRequestDto dto)
        {
            var item = await CurrentUserRestaurant(restaurantId).SelectMany(r => r.MenuItems).SingleOrDefaultAsync(i => i.Id == itemId);
            if (item == null)
                throw new Exception("Item Inválido");

            _mapper.Map(dto, item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRestaurantItem(int restaurantId, int itemId)
        {
            var item = await CurrentUserRestaurant(restaurantId).SelectMany(u => u.MenuItems).SingleOrDefaultAsync(i => i.Id == itemId);
            if (item == null)
                throw new Exception("Item Inválido");

            _context.MenuItems.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
