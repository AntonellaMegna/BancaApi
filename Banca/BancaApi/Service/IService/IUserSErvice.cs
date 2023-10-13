using BancaApi.Models;

namespace BancaApi.Service.IService
{
    public interface IUserService
    {
        Task<User> AddUser(User user);
        Task DeleteUser(string user);
        Task<User?> GetUser(string? userName, string? email);
        Task UpdateUser(User user);
    }
}