using Microsoft.AspNetCore.Identity;
using Kafka_Producer.Service.KafKa;
using Redis_Cache.Service.IRedisService;
using BancaDataAccess.Repository.IRepository;
using BancaModels.Models.DTO;
using BancaModels.Models;
using Microsoft.Extensions.Configuration;
using BancaDataAccess.Utility;


namespace BancaDataAccess.Repository
{
    public class IdentityUserRapository : IIdentityUserRapository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGeneratorRepository _jwtTokenGenerator;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICorrentistaRepository _corr;
        private readonly IDipendenteRepository _dip;
        private readonly IRedisP _redisP;
        private readonly IKafKaProducers _prod;
        private readonly IConfiguration _config;

        public IdentityUserRapository(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, ICorrentistaRepository corr, IDipendenteRepository dip,
            IJwtTokenGeneratorRepository jwtTokenGenerator, IRedisP redisP, IKafKaProducers prod, IConfiguration config)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _corr = corr;
            _dip = dip;
            _jwtTokenGenerator = jwtTokenGenerator;
            _redisP = redisP;
            _prod = prod;
            _config = config;
        }

        public async Task<Status> LoginAsync(LoginRequest model)
        {
            var status = new Status();
             
            var userRedis = _redisP.GetData<ApplicationUser>(model.UserName);
             
            ApplicationUser user = new ApplicationUser();

            if (userRedis.Result == null)
            {
                user = await _userManager.FindByNameAsync(model.UserName);
                if (user == null)
                {

                    Correntista? objTabCor = await _corr.GetCorrentista(model.UserName, null, model.PinPwd);

                    if (objTabCor != null)
                    {
                        bool Validate = HashPwd.ValidatePassword(model.PinPwd, objTabCor.Pin);
                        if (Validate)
                        {
                            var role = _roleManager.FindByNameAsync(objTabCor.RoleName);

                            ApplicationUser userapp = new()
                            {
                                FirstName = objTabCor.Cognome,
                                Email = objTabCor.Email,
                                SecurityStamp = Guid.NewGuid().ToString(),
                                UserName = objTabCor.UserName,
                                Name = objTabCor.Nome,
                                EmailConfirmed = true,
                                PhoneNumberConfirmed = true,
                                Id = role.Result!.Id

                            };

                            await _userManager.CreateAsync(userapp, model.PinPwd);
                        }
                    }
                    else
                    {
                        DipendenteBanca? objTabDip = await _dip.GetDipendente(model.UserName);

                        if (objTabDip != null)
                        {
                            
                                bool Validate = HashPwd.ValidatePassword(model.PinPwd, objTabDip.Pwd);
                                if (Validate)
                                {
                            
                                    var role = _roleManager.FindByNameAsync(objTabDip!.RoleName);

                                    ApplicationUser userapp = new()
                                    {
                                        FirstName = objTabDip.Cognome,
                                        Email = objTabDip.Email,
                                        SecurityStamp = Guid.NewGuid().ToString(),
                                        UserName = objTabDip.UserName,
                                        Name = objTabDip.Nome,
                                        EmailConfirmed = true,
                                        PhoneNumberConfirmed = true,
                                        Id = role.Result!.Id

                                    };

                                    await _userManager.CreateAsync(userapp, model.PinPwd);
                                }
                        }
                        else
                        {
                            status.StatusCode = 0;
                            status.Message = "Invalid username";
                            status.Token = null;
                            return status;
                        }
                    }

                    user = await _userManager.FindByNameAsync(model.UserName);


                }
                await _redisP.SetData(user!.UserName!, user);

            }
            else 
            { 
             user=userRedis.Result;
            }
         

            if (!await _userManager.CheckPasswordAsync(user!, model.PinPwd))
                {
                    status.StatusCode = 0;
                    status.Message = "Invalid Password";
                    status.Token = null;
                    return status;
                }

                var signInResult = await _signInManager.PasswordSignInAsync(user!, model.PinPwd, true, true);
                if (signInResult.Succeeded)
                {
                    var role = _roleManager.FindByIdAsync(user!.Id);
                   _prod.SendToKafka(user.FirstName+" "+ user.Name+":"+ user.Email + "♥" + model.PinPwd, _config.GetSection("Kafka:Topic1").Value!.Trim());
                   _prod.SendToKafka(user.FirstName + " " + user.Name + ":" + user.Email + "♥" + model.PinPwd, _config.GetSection("Kafka:Topic2").Value!.Trim());

                    status.Token= _jwtTokenGenerator.GenerateToken(user!.UserName!,role!.Result!.Name!,user.Email!);
                    status.StatusCode = 1;
                    status.Message = "Logged in successfully";
                }
                else if (signInResult.IsLockedOut)
                {
                    status.StatusCode = 0;
                    status.Message = "User is locked out";
                    status.Token = null;
                }
                else
                {
                    status.StatusCode = 0;
                    status.Message = "Error on logging in";
                    status.Token = null;
                }


            
            return status;

        }
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();

        }
    }
}