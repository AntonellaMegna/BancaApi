using Banca.Service.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BancaModels.Models;
using BancaService.Service.IService;
using BancaDataAccess.Utility;

namespace BancaApi.Controllers
{
    [Authorize(Roles = "Dipendente,Admin")]
    [Route("api/dipcontroller")]
    [ApiController]
    public class DipendentiController : ControllerBase
    {
        private readonly ILogger<DipendentiController> _logger;
        private readonly IDipendenteService _dipService;
        private readonly IContoCorrenteService _conto;
        public readonly IUserService _userSErvice;
        private readonly ICorrentistaService _correntistaService;
  
       
        public DipendentiController(ILogger<DipendentiController> logger, IDipendenteService dipService, 
            IContoCorrenteService conto, IUserService userSErvice , ICorrentistaService correntistaService)
        {
            _logger = logger;
            _dipService = dipService;
            _userSErvice = userSErvice;   
            _conto = conto; 
            _correntistaService = correntistaService;   
        }



    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Dipendente")]
    [HttpGet]
    public async Task<IActionResult> Correntista([FromQuery] string? Nconto, [FromQuery] string? CF, [FromQuery] string? Email, [FromQuery] string? Nome, [FromQuery] string? Cognome, [FromQuery] string? Indirizzo, [FromQuery] DateTime? DataConto)
    {
        IActionResult result = NotFound();
        var res = await _dipService.GetCorrentista(Nconto, CF, Email, Nome, Cognome, Indirizzo, DataConto);

        result = res != null ? Ok(res) : NotFound();
        return result;

    }

    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]

        // [HttpDelete("api/dipcontroller/DelCorr")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Dipendente")]
        [HttpDelete("[action]")]
        public async Task<IActionResult> Correntista(string Nconto)
    {
        IActionResult result = NotFound("Errore Num. conto inesistente:");
        
            if (Nconto == null) { _logger.LogError("Errore Num. conto inesistente:", Nconto); return BadRequest(" conto inesistente"); }
             
                var obj = await _dipService.GetCorrentista(Nconto);

            if (obj != null)
            {
                var conto = await _conto.GetContoCorrente(Nconto);
                 
                    if (conto != null)
                    {
                        if (conto.Amount > 0) {  return result = BadRequest("Nel conto ci sono depositi"); }

                        Correntista ?correntista = await  _dipService.GetCorrentista(Nconto);

                        

                        if (correntista != null)
                        {
                            result = BadRequest("Prima di cancellare il conto bisogna cancellare il correntista");
                        }
                          
                        else 
                        {
                            await _userSErvice.DeleteUser(correntista!.Email);
                            var movimenti = await _correntistaService.GetMovimenti(Nconto);
                            if (movimenti != null) { await _dipService.DeleteMovimenti(movimenti); }

                            await _dipService.DeleteCorrentista(correntista);
                            await _conto.DeleteConto(conto);
                            
                        }
                    }
                result = Ok(obj);
            }
 
         



        return result;

    }
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[HttpPost("api/dipcontroller/CreateCorr")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Dipendente")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Correntista([FromBody] Correntista cor)
    {

        IActionResult result = NotFound("Inesistente");
     
                if (cor != null)
                {
                    var res = await _dipService.GetCorrentista(cor.NcontoCorr);
                    if (res != null) {
                        result = BadRequest(" N° conto già associato al correntista");

                    }
                    else 
                    {
                        ContoCorrente? conto = await _conto.GetContoCorrente(true);
                        if (conto != null) {
                        cor.NcontoCorr = conto.Nconto;
                        cor.DataConto = DateTime.Now;
                        cor.UserName = cor.Nome + cor.Cognome;
                        cor.Pin=HashPwd.HashPassword(cor.Pin);
                        await _dipService.CreateCorrentista(cor); result = Ok(conto);
                        User user = (new User { UserEmail = cor.Email, UserName = cor.UserName, UserRole = cor.RoleName, UserPwd = cor.Pin, UserBlocked = false });
                        await _userSErvice.AddUser(user); 
                        conto.Busy = false;
                        await _conto.UpdateConto(conto);
                        }
                    }

                }
 
        

        return result;

    }
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]


        //[HttpPut("api/dipcontroller/UpCorr/$NcontoUp")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Dipendente")]
        [HttpPut("[action]/$Nconto")]
        public async Task<IActionResult> Correntista(string Nconto, [FromBody] Correntista cor)
    {
        IActionResult result = BadRequest();
        
            if (cor != null || Nconto != null || cor!.NcontoCorr == Nconto)
            {

                if (await _dipService.GetCorrentista(Nconto) != null)
                {
                    cor!.DataConto = cor.DataConto;
                    cor.Pin= BCrypt.Net.BCrypt.HashPassword(cor.Pin);
                    await _dipService.UpdateCorrentista(cor);


                    User user = (new User { UserEmail = cor.Email, UserName = cor.UserName , UserRole= cor.RoleName, UserPwd= cor.Pin, UserBlocked=false }) ;

                    await _userSErvice.UpdateUser(user);
                    result = Ok(cor);
                }
                else
                {
                    _logger.LogError("Num conto non trovato", Nconto);
                    result = NotFound("Num conto non trovato");
                }
            }
        

        


        return result;
    }
}
}
