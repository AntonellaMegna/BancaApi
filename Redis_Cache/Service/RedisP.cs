using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Distributed;
using Redis_Cache.Service.IRedisService;

namespace Redis_Cache.Service.Redis
{
    public class RedisP : IRedisP
    {
        //private readonly IDatabase _db;
        private readonly IDistributedCache _cache;

        public RedisP(IDistributedCache cache)

        {
            //   ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("127.0.0.1:6379");
            //db = redis.GetDatabase();
            //_connectionMultiplexer = connectionMultiplexer ?? throw new ArgumentNullException(nameof(connectionMultiplexer));
            //db = _connectionMultiplexer.GetDatabase(0);
            _cache = cache;
            //  _db=db;
        }

        public async Task SetData<T>(string key, T data)
        {

            await _cache.SetStringAsync(key, JsonConvert.SerializeObject(data));
            //  redis.Close();
            
        }

        public async Task<T> GetData<T>(string key)
        {

            //var redis = ConnectionMultiplexer.Connect("localhost:8003");
            //IDatabase db = redis.GetDatabase();
            var res = await _cache.GetStringAsync(key);

            // redis.Close();
            if (res == null)
                return default(T)!;
            else
                return JsonConvert.DeserializeObject<T>(res!)!;

        }

        public async Task Remove<T>(string key)
        {
            // await _db.KeyDeleteAsync(key);
            await _cache.RemoveAsync(key);
        }

        //public async Task<bool> Exists<T>(string key)
        //{
        //     return await _db.KeyExistsAsync(key);

        //}
    }
}
