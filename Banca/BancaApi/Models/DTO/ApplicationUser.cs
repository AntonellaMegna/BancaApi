﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BancaApi.Models.DTO
{
    public class ApplicationUser: IdentityUser
    {
       [Required]
        public string Name { get; set; } 
       [Required]  
        public string FirstName { get; set; } 
    }
}
