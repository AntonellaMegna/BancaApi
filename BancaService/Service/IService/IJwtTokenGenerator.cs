namespace BancaService.Service.IService
{
    public interface IJwtTokenGenerator 
    {
        string GenerateToken(string username,string role, string email);
    }
}
