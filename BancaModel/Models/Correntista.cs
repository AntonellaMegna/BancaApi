using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BancaModels.Models
{
    [PrimaryKey(nameof(NcontoCorr), nameof(CF))]
    public class Correntista
    //DateOnly.FromDateTime(DateTime.Now);
    {     
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [MaxLength(30)]
        public string NcontoCorr { get; set; }
        [MaxLength(20)]
        [Required(ErrorMessage = "Il campo CF non può esser vuoto")]
        // [RegularExpression("^[A-Z]{6}[\\d]{2}[A-Z][\\d]{2}[A-Z][\\d]{3}[A-Z]$", ErrorMessage ="Codice fiscale non valido")]
       
        public string CF { get; set; }

        [EmailAddress(ErrorMessage = " Email non valida")]
        [Required(ErrorMessage = "Il campo Email non può esser vuoto")]
           public string Email { get; set; }

        [Required(ErrorMessage = "Il campo Pin non può esser vuoto")]
          public string Pin { get; set; }

        [Required(ErrorMessage = "Il campo Nome non può esser vuoto")]
        [MaxLength(30)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Il campo Cognome non può esser vuoto")]
        [MaxLength(30)]
        public string Cognome { get; set; }

        [Required(ErrorMessage = "Il campo Indirizzo non può esser vuoto")]
        [MaxLength(50)]
        public string Indirizzo { get; set; }
        [Required(ErrorMessage = "Il campo Num Conto non può esser vuoto")]


        // [ConcurrencyCheck]
        // [JsonIgnore]
        //[ForeignKey(nameof(NcontoCorr))]
        [ForeignKey("NcontoCorr")]
         public ContoCorrente NumConto { get; set; }
      //  public ICollection<ContoCorrente> NumConto { get; set; }

        [Required(ErrorMessage = "Il campo Data non può esser vuoto")]
        public DateTime DataConto { get; set; }

        [Required(ErrorMessage = "Il campo UserName non può esser vuoto")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Il campo Ruolo non può esser vuoto")]
        public string RoleName { get; set; }

      
    }
}
