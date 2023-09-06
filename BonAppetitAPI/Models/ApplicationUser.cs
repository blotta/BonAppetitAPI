using Microsoft.AspNetCore.Identity;

namespace BonAppetitAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() : base() { }

        public string Name { get; set; }

        public ICollection<Restaurant> Restaurants { get; set;} = new List<Restaurant>();
    }
}
