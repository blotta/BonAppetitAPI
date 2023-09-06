using BonAppetitAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace BonAppetitAPI.Data.Dtos.Profile
{
    public class MenuItemReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }

    public class MenuItemCreateRequestDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Description { get; set; }
    }

    public class MenuItemUpdateRequestDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Description { get; set; }
    }

    public class MenuItemProfile : AutoMapper.Profile
    {
        public MenuItemProfile()
        {
            CreateMap<MenuItemCreateRequestDto, MenuItem>();
            CreateMap<MenuItemUpdateRequestDto, MenuItem>();

            CreateMap<MenuItem, MenuItemReadDto>();
        }
    }
}
