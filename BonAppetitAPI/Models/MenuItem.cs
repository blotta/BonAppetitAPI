using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BonAppetitAPI.Models
{
    public class MenuItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        [Precision(14, 2)]
        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public Restaurant Restaurant { get; set; }

        public ICollection<MenuSection> MenuSections { get; set; } = new List<MenuSection>();
    }
}
