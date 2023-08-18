﻿using Microsoft.AspNetCore.Identity;

namespace BonAppetitAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() : base() { }

        public string Name { get; set; }
    }
}
