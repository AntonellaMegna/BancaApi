using BancaApi.Data;
using BancaApi.Models;
using BancaApi.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BancaApi.Repository
{
    public class UserRepository : IUserRepository
    {
        protected readonly AppDbContext _db;
        public UserRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<User> AddUser(User user)
        {
            _db.Add(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task DeleteUser(string user)
        {
            _db.Remove(user);
            await _db.SaveChangesAsync();

        }

        public async Task<User?> GetUser(string userName, string email)
        {
            var query = await _db.User
              .Where(st => st.UserName == (userName ?? st.UserName))
              .Where(st => st.UserEmail == (email ?? st.UserEmail))
              .FirstOrDefaultAsync();
            return query;
        }


        public async Task UpdateUser(User user)
        {
            _db.Update(user);
            await _db.SaveChangesAsync();
        }
    }
}
