using BancaDataAccess.Repository.IRepository;
using BancaModels.Models.DTO;
using BancaService.Service.IService;

namespace BancaService.Service
{

    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<Status> Login(LoginRequest request)
        {
            var status = new Status();
            if (request != null)
            {
                status = await _authRepository.Login(request);
            }
            else
            {
                status.StatusCode = 0;
            }

            return status;

        }
    }
}
