using BonAppetitAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BonAppetitAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuSection> MenuSections { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
    }
}
