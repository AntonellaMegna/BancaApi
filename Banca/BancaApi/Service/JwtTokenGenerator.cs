
using BancaApi.Service.IService;
using BancaApi.Utility;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BancaApi.Service
{
    public class JwtTokenGenerator :  IJwtTokenGenerator
    {
        private readonly JwtOptions _jwtOptions;
      
        public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
             
        }


        public string GenerateToken(string username, string role, string email)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            
            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value!));
            //var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
            //_configuration.GetSection("AppSettings:Token").Value));

            var key = Encoding.ASCII.GetBytes(_jwtOptions.Key);
             
            //var claimList = new List<Claim>
            //{
            //    new Claim(JwtRegisteredClaimNames.Email,user.UserEmail),
            //    new Claim(JwtRegisteredClaimNames.Name,user.UserName)

            //};
            
            

            List<Claim> claimList = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,Guid.NewGuid().ToString()),// crea un id che identifica tutti i dati utente
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
                 new Claim(ClaimTypes.Email, email),
                // new Claim(JwtRegisteredClaimNames.Email,user.UserEmail),

            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer,
                Subject = new ClaimsIdentity(claimList),
                //Expires = DateTime.UtcNow.AddMinutes(3),
                Expires = DateTime.Now.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

         


        //public string GenerateToken(User user)
        //{
        //    List<Claim> claimList = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.Name, user.UserName),
        //        new Claim(ClaimTypes.Role, user.UserRole)
        //    };

        //    var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
        //        _configuration.GetSection("ApiSettings:Key").Value));

        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        //    var token = new JwtSecurityToken(
        //        claims: claimList,
        //        expires: DateTime.Now.AddDays(1),
        //        //audience: _configuration.GetSection("ApiSettings:Audience").Value,
        //        issuer: _configuration.GetSection("ApiSettings:Issuer").Value,
        //        signingCredentials: creds);

        //    var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        //    return jwt;
        //}


    }
}
