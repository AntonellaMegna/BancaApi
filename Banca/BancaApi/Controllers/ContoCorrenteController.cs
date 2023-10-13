using Banca.Service.Service.IService;
using BancaModels.Models;
using BancaService.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace BancaApi.Controllers
{
    [Authorize(Roles = "Dipendente,Admin")]
    [Route("api/contocontroller")]
    [ApiController]
    
    public class ContoCorrenteController : ControllerBase
    {
        private readonly ILogger<ContoCorrenteController> _logger;
        private readonly IContoCorrenteService _conto;
    
        public ContoCorrenteController(ILogger<ContoCorrenteController> logger, IContoCorrenteService conto)
        {
            _logger = logger;
            _conto = conto;
           
           
        }
            [ProducesResponseType(StatusCodes.Status404NotFound)]
        //    [HttpGet("{Nconto}", Name = "GetGetByNconto")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Dipendente,Admin")]
        [HttpGet("[action]/{Nconto}")]
        public async Task<IActionResult> ContoCorrente(string Nconto)
        {
                   IActionResult result ;

                  if (Nconto == null)
                   {
                       _logger.LogError("Errore ", "valore nullo");
                       return BadRequest("Il valore non può esser nullo");
                   }

            var res = await _conto.GetContoCorrente(Nconto);
               result = res != null ? Ok(res) : NotFound("Conto non trovato");

               return result;
        }

            

       
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Dipendente,Admin")]
        [HttpPost("[action]")]
        public async Task<ActionResult> ContoCorrente([FromBody] ContoCorrente conto)
        {
            IActionResult result=NotFound();

         
                if (conto==null) { return BadRequest(); }

                var res = await _conto.GetContoCorrente(conto.Nconto);

                if (res == null)
                {
                    _logger.LogCritical("Errore", "Num Conto presente nel db");
                    
                }
                else
                {
                  await _conto.CreateConto(conto); 
                  result =Ok( res); 
                }
               
          
            


            return Ok(result);

        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        //  [HttpDelete("{NcontoDel}", Name = "DelCC")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Dipendente,Admin")]
        [HttpDelete("[action]/{nconto}")]
        public async Task<IActionResult> DelContoCorrente(string  nconto)
        {
            IActionResult result = NotFound("");
           
                if (nconto == null ) 
                {
                    _logger.LogError("Num conto inesistente", nconto);
                    return BadRequest();
                }
                ContoCorrente? obj = await _conto.GetContoCorrente(nconto);
                 
                if (obj != null) { await _conto.DeleteConto(obj); } else { result=BadRequest("Conto inesistente"); }
         


            return result;
       
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]


        //  [HttpPut("{NcontoUp}", Name = "UpdateConto")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Dipendente,Admin")]
        [HttpPut("[action]/{nconto}")]
        public async Task<IActionResult> ContoCorrente( string nconto, [FromBody] ContoCorrente contocorrente) {
           
              IActionResult result = NotFound();
          
                if (nconto == null || nconto != contocorrente.Nconto) { _logger.LogError("Errore", "record non trovato"); result = BadRequest(); }
                else
                {
                    if ( await _conto.GetContoCorrente(nconto) != null)
                    {
                        await _conto.UpdateConto(contocorrente); result = Ok(contocorrente);
                    }
                }
              

               
          
            return result;
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Dipendente,Admin")]
        [HttpGet("[action]")]
        public async Task<IActionResult> ContoCorrente([FromQuery] string? Nconto, string? Iban,  string ? Ncarta)
        {
            IActionResult result=BadRequest();
            if (Nconto != null && Iban == null && Ncarta == null)
            {
                result = BadRequest("Errore: almeno uno dei campi richiesti deve esser valorizzato");

            }
            else
            {
                var res = await _conto.GetContoCorrente(Nconto, Iban, Ncarta);
                result = res == null?  result = NotFound("Nessun record trovato"): result = Ok(res);
            }
             
         

            return result;
        }
       
      
   
    }
}
