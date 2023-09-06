using AutoMapper;
using BonAppetitAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace BonAppetitAPI.Data.Dtos.Profile
{
    public class RestaurantReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }

    public class RestaurantCreateRequestDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
    }

    public class RestaurantUpdateRequestDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
    }

    public class RestaurantProfile : AutoMapper.Profile
    {
        public RestaurantProfile()
        {
            CreateMap<RestaurantCreateRequestDto, Restaurant>();
            CreateMap<RestaurantUpdateRequestDto, Restaurant>();

            CreateMap<Restaurant, RestaurantReadDto>();
        }
    }
}
