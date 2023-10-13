using BancaApi.Models;
using BancaApi.Models.DTO;

namespace BancaApi.Service.IService
{
    public interface IJwtTokenGenerator 
    {
        string GenerateToken(string username,string role, string email);
    }
}
