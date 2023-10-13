using BancaModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Redis_Cache.Service.IRedisService;
using System.Security.Claims;
using BancaService.Service.IService;
using BancaDataAccess.Utility;

namespace BancaApi.Controllers
{
    [Authorize(Roles = "Dipendente,Admin,Correntista")]
    [Route("api/corrcontroller")]
    [ApiController]
    public class CorrentistaController : ControllerBase
    {
        private readonly ICorrentistaService _correntista;
        private protected IContoCorrenteService _conto;
        private readonly ILogger<CorrentistaController> _logger;
        public readonly IUserService _userSErvice;
        private readonly IRedisP _redisP;


        //  private readonly IMapper _mapper;
        public CorrentistaController(ICorrentistaService correntista, ILogger<CorrentistaController> logger, IContoCorrenteService conto, IUserService userSErvice, IRedisP redisP)
        {
            _correntista = correntista;
            _logger = logger;
            _conto = conto; 
            _userSErvice = userSErvice; 
            _redisP = redisP;
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]


        //  [HttpGet("api/corrcontroller/GetGetByData"), Authorize(Roles = "Correntista")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Correntista")]
        [HttpGet("[action]")]//, Authorize(Roles = "Correntista")]
        public async Task<IActionResult> Movimenti([FromQuery]  DateTime? start,  DateTime? end,  string? tipo)
        {
            IActionResult result = BadRequest();
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;

            //      if (Nconto == null && start.HasValue && end.HasValue) { return result;}
            if (userName == null) { return result; }

            Correntista? cor = await _correntista.GetCorrentista(userName, null,null);

            if (cor == null) { return NotFound("correntista inesistente"); }
            var res = await _correntista.GetMovimenti(cor.NcontoCorr, start, end, tipo);

            return Ok(res);
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Correntista")]
        [HttpPost("[action]")]
       public async Task<IActionResult>Movimento(string Tipo, int amount)
        {
            IActionResult result = NotFound();
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
           
                if (email != null && Tipo != null && (Tipo.ToUpper() == "P" || Tipo!.ToUpper()== "D"))
                {
                    Correntista? res = await _correntista.GetCorrentista(null,email!,null);
                    if (res != null)
                    {
                        ContoCorrente? conto = await _conto.GetContoCorrente(res.NcontoCorr);

                        if (Tipo.ToUpper() == "D") { conto!.Amount += amount; } else { conto!.Amount -= amount; }

                        var mov = new Movimenti() { TipoMov = Tipo, Amount = amount, Nconto = res.NcontoCorr, NumConto = res.NumConto, DataContoMov= DateTime.Now};
                       

                        result = await _correntista.CreateMovimenti(mov) != null ? Ok(await _conto.UpdateConto(conto)) : BadRequest($"{Tipo} non riuscito"); 
                     
 
                    }

                }
            

            return result;

        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Correntista")]
        [HttpPut("[action]")]//, Authorize(Roles = "Correntista")]
        public async Task<IActionResult> Correntista(string pinOld,  string newPin)
        {
            IActionResult result = NotFound();

           
                var userName = User.FindFirst(ClaimTypes.Name)?.Value;
                if (userName != null && pinOld != null && newPin != null)
                {
            
                    Correntista? cor = await _correntista.GetCorrentista(userName, null,null);
                    if (cor != null)
                    {
                        bool Validate = HashPwd.ValidatePassword(pinOld, cor!.Pin);
                            if (!Validate) 
                            {
                                result= BadRequest("Pwd errata"); 
                        
                            } else
                            {
                                newPin = HashPwd.HashPassword(newPin);
                                cor.Pin = newPin;
                                var ok = await _correntista.UpdateCorrentista(cor);

                                    if (ok == null) 
                                    {
                                    result = BadRequest("La Modifica del Pin non è andata a buon fine ");
                                    }
                                    else
                                    {
                                        User? user = await _userSErvice.GetUser(userName, null);

                                        user!.UserPwd = newPin;
                                        await _userSErvice.UpdateUser(user);
                                        result = Ok("Modifica effettuata");
                                        var userRedis = _redisP.GetData<User>(user!.UserName);
                                        if (userRedis != null)
                                        {
                                         await _redisP.SetData(user.UserName, newPin!);
                                        }


                        }
                            }
                    }
                
                    else 
                    {
                      
                        result = BadRequest("correntista inesistente" ); 
                    }
 
                }
         
            return result;

        }



    }
}
