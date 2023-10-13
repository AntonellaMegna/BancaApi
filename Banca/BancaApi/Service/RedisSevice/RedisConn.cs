using StackExchange.Redis;

namespace BancaApi.Service.Redis
{
    public static class RedisConn
    {
        public static ConnectionMultiplexer ConnectRedis(IConfiguration Configuration)
        {

            var redisOptions = ConfigurationOptions.Parse($"{Configuration["Redis:Host"]}:{Configuration["Redis:Port"]}");
            redisOptions.Password = Configuration["Redis:Password12345"];
            redisOptions.AbortOnConnectFail = false;
            var multiplexer = ConnectionMultiplexer.Connect(redisOptions);

            return multiplexer;
        }
    }
}
