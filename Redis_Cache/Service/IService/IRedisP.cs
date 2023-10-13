namespace Redis_Cache.Service.IRedisService
{
    public interface IRedisP
    {
        public Task SetData<T>(string key, T data);
        public Task<T> GetData<T>(string key);
        public Task Remove<T>(string key);
        // public Task<bool> Exists<T>(string key);

    }
}
