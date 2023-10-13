
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BancaModels.Models
{
   
    public class Movimenti
    {
        [Key]
        public int IdM { get; set; }

        [Required(ErrorMessage = "Il campo Num Conto non può esser vuoto")]
        [MaxLength(30)]
        public string Nconto { get; set; }
        
      
        [Required(ErrorMessage = "Il campo Tipo Mov non può esser vuoto")]
        [MaxLength(10)]
        public string TipoMov { get; set; }
        [Required(ErrorMessage = "Il campo Amount non può esser vuoto")]
        public decimal Amount { get; set; }
       
        [ForeignKey("Nconto")]
              
        //    [JsonIgnore]
         public ContoCorrente NumConto { get; set; }
       // public ICollection<Correntista> NumConto { get; set; }
        [Required(ErrorMessage = "Il campo Data non può esser vuoto")]
        public DateTime DataContoMov { get; set; }
       
    }
}

