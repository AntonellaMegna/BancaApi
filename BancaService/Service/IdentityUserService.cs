using BancaDataAccess.Repository.IRepository;
using BancaModels.Models.DTO;
using BancaService.Service.IService;

namespace BancaService.Service
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
