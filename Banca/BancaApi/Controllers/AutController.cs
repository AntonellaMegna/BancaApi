using BancaApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BancaApi.Controllers: ControllerBase
{

    [Route("api/[autcontroller]")]
    [ApiController]
    public class AutController : ControllerBase
    { public static User user = new User();

        [HttpPost("register")]
        public ActionResult<User> Register(UserRequest request) {

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.UserName= request.UserName;    
            user.Password=passwordHash; 
            return Ok(user);

        
        }

    }
}
