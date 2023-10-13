using System.ComponentModel.DataAnnotations;

namespace BancaModels.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        [EmailAddress]

        public string UserEmail { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string UserPwd { get; set; }
        [Required]
        public string UserRole { get; set; }
        [Required]
        public bool UserBlocked { get; set; } = false;
    }
}
