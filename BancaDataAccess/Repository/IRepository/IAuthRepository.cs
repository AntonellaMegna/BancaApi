using BancaModels.Models.DTO;

namespace BancaDataAccess.Repository.IRepository
{
    public interface IAuthRepository
    {
        Task<Status> Login(LoginRequest request);
    }
}