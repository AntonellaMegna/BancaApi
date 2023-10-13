
using BancaModels.Models.DTO;

namespace BancaService.Service.IService
{
    public interface IIdentityUserService
    {
        Task<Status> LoginAsync(LoginRequest model);
        Task LogoutAsync();
    }
}
