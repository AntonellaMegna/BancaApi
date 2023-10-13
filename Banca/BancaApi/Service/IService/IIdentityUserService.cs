using BancaApi.Models.DTO;
using BancaApi.Utility;

namespace BancaApi.Service.IService
{
    public interface IIdentityUserService
    {
        Task<Status> LoginAsync(LoginRequest model);
        Task LogoutAsync();
    }
}
