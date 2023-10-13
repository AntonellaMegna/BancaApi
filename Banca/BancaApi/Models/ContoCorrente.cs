using System.ComponentModel.DataAnnotations;

namespace BancaApi.Models
{
    public class ContoCorrente
    {
        [Key]
        [Required(ErrorMessage = "Il campo Num Conto non può esser vuoto")]
        [MaxLength(30)]
        public string  Nconto { get; set; }

        [Required(ErrorMessage ="Il campo IBAN non può esser vuoto")]
        // [RegularExpression("^([A-Z]{2}[ \-]?[0-9]{2})(?=(?:[ \-]?[A-Z0-9]){9,30}$)((?:[ \-]?[A-Z0-9]{3,5}){2,7})([ \-]?[A-Z0-9]{1,3})?$", ErrorMessage ="Iban non valido")]
        [MaxLength(30)]
        public string Iban { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Il campo Num Carta non può esser vuoto")]
        [MaxLength(20)]
        public string Ncarta { get; set; }

        [Required(ErrorMessage = "Il campo Busy non può esser vuoto")]
        public bool Busy { get; set; }
         

     
    }
}
