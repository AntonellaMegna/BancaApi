using BancaDataAccess.Repository.IRepository;
using BancaDataAccess.Utility;
using BancaModels.Models;
using BancaModels.Models.DTO;
using Kafka_Producer.Service.KafKa;
using Microsoft.Extensions.Configuration;
using Redis_Cache.Service.IRedisService;


namespace BancaDataAccess.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IUserRepository _userRep;
        private readonly IJwtTokenGeneratorRepository _jwtTokenGenerator;
        private readonly ICorrentistaRepository _corr;
        private readonly IDipendenteRepository _dip;
        private readonly IRedisP _redisP;
        private readonly IKafKaProducers _prod;
        private readonly IConfiguration _config;


        public AuthRepository(IUserRepository userRep, IJwtTokenGeneratorRepository jwtTokenGenerator, ICorrentistaRepository corr,
           IDipendenteRepository dip, IRedisP redisP, IKafKaProducers prod, IConfiguration config)
        {
            _config = config;
            _userRep = userRep;
            _jwtTokenGenerator = jwtTokenGenerator;
            _corr = corr;
            _dip = dip;
            _prod = prod;
            _redisP = redisP;
        }


        public async Task<Status> Login(LoginRequest request)
        {
            var status = new Status();


            User user = new User();
            var userRedis = _redisP.GetData<User>(request.UserName);
            var pwd = string.Empty;

            if (userRedis.Result == null)
            {
                user = await _userRep.GetUser(request.UserName!, null);

                if (user == null)
                {

                    Correntista? objTabCor = await _corr.GetCorrentista(request.UserName, null, null);

                    if (objTabCor != null)
                    {
                        bool ValidateCor = HashPwd.ValidatePassword(request.PinPwd, objTabCor.Pin);

                        if (!ValidateCor)
                        {
                            status.StatusCode = 0;
                            status.Message = "Password errata";
                            status.Token = null;

                            return status;
                        }
                        else
                        {

                            User users = new User { UserEmail = objTabCor.Email, UserName = objTabCor.UserName, UserRole = objTabCor.RoleName, UserPwd = objTabCor.Pin, UserBlocked = false };
                            user = users;
                        }

                    }
                    else
                    {
                        DipendenteBanca? objTabDip = await _dip.GetDipendente(request.UserName);

                        if (objTabDip == null)
                        {
                            status.StatusCode = 0;
                            status.Message = "User Inesistente";
                            status.Token = null;

                            return status;

                        }
                        else
                        {
                            bool ValidateDip = HashPwd.ValidatePassword(request.PinPwd, objTabDip.Pwd);

                            if (!ValidateDip)
                            {
                                status.StatusCode = 0;
                                status.Message = "Password errata";
                                status.Token = null;

                                return status;
                            }
                            else
                            {
                                User users = new User { UserEmail = objTabDip.Email, UserName = objTabDip.UserName, UserRole = objTabDip.RoleName, UserPwd = objTabDip.Pwd, UserBlocked = false };
                                user = users;
                            }
                        }

                    }

                    await _userRep.AddUser(user);
                    pwd = user.UserPwd;

                }

                await _redisP.SetData(user.UserName, user);
                pwd = user.UserPwd;
            }
            else
            {
                pwd = userRedis.Result.UserPwd;
                user = userRedis.Result;
            }

            bool Validate = HashPwd.ValidatePassword(request.PinPwd, pwd);

            if (!Validate)
            {
                status.StatusCode = 0;
                status.Message = "Password errata";
                status.Token = null;


            }
            else
            {
                _prod.SendToKafka(user.UserEmail + "♥" + request.PinPwd, _config.GetSection("Kafka:Topic1").Value!.Trim());
                _prod.SendToKafka(user.UserEmail + "♥" + request.PinPwd, _config.GetSection("Kafka:Topic2").Value!.Trim());
                status.Token = _jwtTokenGenerator.GenerateToken(user.UserName, user.UserRole, user.UserEmail);
                status.StatusCode = 1;
                status.Message = "Logged in successfully";
            }





            return status;
        }
    }
}
