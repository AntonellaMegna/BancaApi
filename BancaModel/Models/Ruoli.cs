﻿using System.ComponentModel.DataAnnotations;

namespace BancaModels.Models
{
    public class Ruoli
    {
        [Key]
        [Required]
        public int RoleId { get; set; }
        [Required(ErrorMessage = "Il campo Nome ruolo non può esser vuoto")]
        public string RoleName { get; set; }
    }
}
