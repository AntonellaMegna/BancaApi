
using Banca.Service.Service.IService;
using Microsoft.AspNetCore.Mvc;
using BancaModels.Models.DTO;
using BancaService.Service.IService;

namespace BancaApi.Controllers
{
    [Route("api/userauthcontroller")]
    [ApiController]
    public class UserAuthenticationController : ControllerBase
    {
        private readonly IIdentityUserService _authService;
        private readonly ILogger<UserAuthenticationController> _logger;
        

        public UserAuthenticationController(IIdentityUserService authService, ILogger<UserAuthenticationController> logger)
        {
            _authService = authService;
            _logger = logger;
              
        }


        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("LoginUser")]
        public async Task<IActionResult> LoginUser(LoginRequest model)
        {
            string token = string.Empty;
            if (ModelState.IsValid) 
              {
              var status= await _authService.LoginAsync(model);
                if (status.StatusCode > 0 && status.Token != null)
                {
                    
                    token = status.Token;
                }
                else 
                {
                    _logger.LogError(status.Message);
                    return BadRequest(status.Message) ;
                }
              }

            LoginResponse loginResponse = new ()
            {
                Token = token
            };

            return Ok(loginResponse);
        }
    }

}
