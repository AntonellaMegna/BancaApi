
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BancaModels.Models
{
    public class DipendenteBanca
    {
        [Key]

        [EmailAddress(ErrorMessage = " Email non valida")]
        [Required(ErrorMessage = "Il campo Email non può esser vuoto")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Il campo Nome non può esser vuoto")]
        [MaxLength(30)]
        [DisplayName("Nome")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Il campo Cognome non può esser vuoto")]
        [MaxLength(30)]
        [DisplayName("Cognome")]
        public string Cognome { get; set; }
        [Required(ErrorMessage = "Il campo Pwd non può esser vuoto")]
    
        [DisplayName("Password")]
        public string Pwd { get; set; }

        [Required(ErrorMessage = "Il campo UserName non può esser vuoto")]
     
        [MaxLength(60)]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Il campo Ruolo non può esser vuoto")]
        [MaxLength(20)]
        public string RoleName { get; set; }
       


    }
}
