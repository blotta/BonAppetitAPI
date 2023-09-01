using System.ComponentModel.DataAnnotations;

namespace BonAppetitAPI.Models
{
    public class Menu
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        public string Name { get; set; }

        public int RestaurantId { get; set; }

        [Required]
        public Restaurant Restaurant { get; set; }

        public ICollection<MenuSection> Sections { get; set; } = new List<MenuSection>();
    }
}
