using BancaApi.Models.DTO;
using BancaApi.Repository.IRepository;
using BancaApi.Service.IService;
using BancaApi.Utility;

namespace BancaApi.Service
{
    public class IdentityUserService : IIdentityUserService
    {
        private readonly IIdentityUserRapository _rapository;
        public IdentityUserService(IIdentityUserRapository rapository) 
        { 
         _rapository = rapository;
        }
        public async Task<Status> LoginAsync(LoginRequest model)
        {
            var status = new Status();
            if (model != null)
            {
                status = await _rapository.LoginAsync(model);
            }
            else
            {
                status.StatusCode = 0;
            }

            return status;

        }

        public async Task LogoutAsync()=> await  _rapository.LogoutAsync();
        
    }
}
