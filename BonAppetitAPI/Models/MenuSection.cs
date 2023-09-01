using System.ComponentModel.DataAnnotations;

namespace BonAppetitAPI.Models
{
    public class MenuSection
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string imageUrl { get; set; }

        public ICollection<MenuItem> Items { get; set; } = new List<MenuItem>();
    }
}
