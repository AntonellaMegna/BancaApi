using BancaModels.Models.DTO;

namespace BancaService.Service.IService
{
    public interface IAuthService
    {
        Task<Status> Login(LoginRequest request);
    }
}