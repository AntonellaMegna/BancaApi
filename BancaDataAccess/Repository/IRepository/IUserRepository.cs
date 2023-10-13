using BancaModels.Models;

namespace BancaDataAccess.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<User> AddUser(User user);
        Task DeleteUser(string user);
        Task<User?> GetUser(string userName, string email);
        Task UpdateUser(User user);
    }
}