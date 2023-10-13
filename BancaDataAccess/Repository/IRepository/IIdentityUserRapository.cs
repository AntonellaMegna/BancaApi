

using BancaModels.Models.DTO;

namespace BancaDataAccess.Repository.IRepository
{
    public interface IIdentityUserRapository
    {
        Task<Status> LoginAsync(LoginRequest model);
        Task LogoutAsync();
    }
}