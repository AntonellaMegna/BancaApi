using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using Serilog;
using BancaApi.ActionFilter;
using Microsoft.AspNetCore.Identity;
using Kafka_Producer.Service.KafKa;
using Redis_Cache.Service.IRedisService;
using Redis_Cache.Service.Redis;
using BancaDataAccess.Data;
using Banca.Service.Service.IService;
using BancaService.Service;
using BancaModels.Models.DTO;
using BancaDataAccess.Repository.IRepository;
using BancaDataAccess.Repository;
using BancaService.Service.IService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
 

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// pulsante su swagger per l'autorizzazione
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Inserire bearer  + il token generato",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));



//builder.Services.AddDbContext<AppDbContext>(option => {option.UseSqlServer(builder.Configuration.GetConnectionString("StringaConn")); },ServiceLifetime.Transient);//.ServiceLifetime.Transient
//var dbHost = Environment.GetEnvironmentVariable("MYSQL_HOST");
//var dbName = Environment.GetEnvironmentVariable("MYSQL_DB");
//var dbPwd = Environment.GetEnvironmentVariable("MYSQL_PASSWORD");
//var dbUser = Environment.GetEnvironmentVariable("MYSQL_USER");
var dbHost = builder.Configuration.GetValue<string>("MYSQL_HOST");
var dbName = builder.Configuration.GetValue<string>("MYSQL_DB");
var dbPwd = builder.Configuration.GetValue<string>("MYSQL_PASSWORD");
var dbUser = builder.Configuration.GetValue<string>("MYSQL_USER");


var connectionString = $"Data Source={dbHost},1433;Initial Catalog={dbName};User id={dbUser} ; Password={dbPwd}; Integrated Security=True;TrustServerCertificate=True;Trusted_Connection=False;";

var conn = builder.Configuration["ConnectionStrings:StringaConn"];
if (dbHost != null) {conn = $"server={dbHost} ,1433;user={dbUser};database={dbName};password={dbPwd};TrustServerCertificate=True;"; }
builder.Services.AddDbContext<AppDbContext>(option => { option.UseSqlServer(conn); });

//IConnectionMultiplexer redis = ConnectionMultiplexer.Connect("172.19.80.1");
//builder.Services.AddScoped(s => redis.GetDatabase());

builder.Services.AddStackExchangeRedisCache(options =>
{
    // options.Configuration = "redis_image:6379"; quando compilo con  docker 
    options.Configuration = builder.Configuration.GetSection("Redis:ConfigurationRedis").Value!; // complilo IIS
    options.InstanceName = builder.Configuration.GetSection("Redis:InstanceNameRedis").Value!;
});

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));
var jwtoptions = new JwtOptions();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
               .GetBytes(builder.Configuration.GetSection("ApiSettings:JwtOptions:key").Value!)),
            ValidIssuer = jwtoptions.Key,
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidAudience = jwtoptions.Audience,
            ValidateLifetime = true,
            RequireExpirationTime = true,  // rafforza la scadenza del token impostato
            ClockSkew = TimeSpan.Zero,// se non s'imposta a zero , il token scaduto vive per altre 5 minuti
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExFilter>();
});

// For Identity  
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IDipendenteRepository, DipendenteRepository>();
builder.Services.AddScoped<IContoCorrenteRepository, ContoCorrenteRepository>();
builder.Services.AddScoped<ICorrentistaRepository, CorrentistaRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IJwtTokenGeneratorRepository, JwtTokenGeneratorRepository>();
builder.Services.AddScoped<IIdentityUserRapository, IdentityUserRapository>();
builder.Services.AddScoped<IContoCorrenteService, ContoCorrenteService>();
builder.Services.AddScoped<ICorrentistaService, CorrentistaService>();
builder.Services.AddScoped<IDipendenteService, DipendenteService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IIdentityUserService, IdentityUserService>();
builder.Services.AddScoped<IAuthService, AuthService>();


//builder.Services.AddScoped<MyMiddleware>();
builder.Services.AddScoped<IRedisP, RedisP>();
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddScoped<IKafKaProducers, KafKaProducers>();


//IUserAuthenticationTokenStore
var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   
    app.UseSwagger();
    app.UseSwaggerUI();
   

}
app.UseSerilogRequestLogging();

app.UseRouting();
app.UseAuthentication();

app.UseHttpsRedirection();
app.UseAuthorization();
//ApplyMigration();
app.MapControllers();



app.Run();
//controlla ogni riavvio dell'applicazione se ci sono migrazioni in sospeso e se ci sono le applicherà automaticamente  al db
//void ApplyMigration()
//{
//    using (var scope = app.Services.CreateScope())
//    {
//        var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

//        if (_db.Database.GetPendingMigrations().Count() > 0)
//        {
//            _db.Database.Migrate();
//        }
//    }
//}