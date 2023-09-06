using System.ComponentModel.DataAnnotations;

namespace BonAppetitAPI.Models
{
    public class Restaurant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public ApplicationUser Owner { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        public ICollection<Menu> Menus = new List<Menu>();
        public ICollection<MenuItem> MenuItem = new List<MenuItem>();
    }
}
