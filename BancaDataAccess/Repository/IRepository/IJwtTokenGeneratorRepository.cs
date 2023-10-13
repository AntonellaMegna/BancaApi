
namespace BancaDataAccess.Repository.IRepository
{
    public interface IJwtTokenGeneratorRepository 
    {
        string GenerateToken(string username,string role, string email);
    }
}
