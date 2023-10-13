using BancaApi.Models.DTO;
using BancaApi.Utility;

namespace BancaApi.Repository.IRepository
{
    public interface IIdentityUserRapository
    {
        Task<Status> LoginAsync(LoginRequest model);
        Task LogoutAsync();
    }
}