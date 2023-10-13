using BancaApi.Data;
using BancaApi.Models;
using BancaApi.Repository.IRepository;
using BancaApi.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace BancaApi.Service
{
    public class UserService : IUserService
    {
        protected readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> AddUser(User user) => await _userRepository.AddUser(user);
        public async Task DeleteUser(string user) => await _userRepository.DeleteUser(user);
        public async Task<User?> GetUser(string? userName, string? email) => await _userRepository.GetUser(userName!, email!);

        public async Task UpdateUser(User user) => await _userRepository.UpdateUser(user);

    }
}
