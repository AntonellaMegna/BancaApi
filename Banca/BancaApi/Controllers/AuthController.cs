
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BancaModels.Models.DTO;
using BancaService.Service.IService;


namespace BancaApi.Controllers
{
    [Route("api/authcontroller")]
    [ApiController]
    public class AuthController : ControllerBase
    {
       private readonly ILogger<AuthController> _logger;    
     
        private readonly IAuthService _authService;
        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {
           _logger = logger;
           _authService = authService;
        }
       

        [AllowAnonymous]
     
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {

            string token = string.Empty;
            if (ModelState.IsValid)
            {
                var status = await _authService.Login(request);
                if (status.StatusCode > 0 && status.Token != null)
                {

                    token = status.Token;
                }
                else
                {
                    _logger.LogError(status.Message);
                    return BadRequest(status.Message);
                }
            }

            LoginResponse loginResponse = new()
            {
                Token = token
            };

            return Ok(loginResponse);

        }





    }
}
