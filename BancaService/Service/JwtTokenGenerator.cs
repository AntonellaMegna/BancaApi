using BancaDataAccess.Repository.IRepository;
using BancaService.Service.IService;
 

namespace BancaService.Service
{
    public class JwtTokenGenerator :  IJwtTokenGenerator
    {
        
        private readonly IJwtTokenGeneratorRepository _jwtRep;
          public JwtTokenGenerator(IJwtTokenGeneratorRepository jwtRep)
        {
            _jwtRep = jwtRep;
             
        }


        public string GenerateToken(string username, string role, string email) =>  _jwtRep.GenerateToken(username, role, email);





    }
}
