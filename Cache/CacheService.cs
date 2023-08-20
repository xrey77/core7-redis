using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using core7_redis.Cache;

namespace core7_redis.Cache
{
    public class CacheService: ICacheService {

        // var configs = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false).Build();
        // Console.WriteLine("Redis : " + configs);

        private IDatabase _db;
        public CacheService() {
            ConfigureRedis();
        }
        private void ConfigureRedis() {
            _db = ConnectionHelper.Connection.GetDatabase();
        }
        public T GetData < T > (string key) {
            var value = _db.StringGet(key);
            if (!string.IsNullOrEmpty(value)) {
                return JsonConvert.DeserializeObject < T > (value);
            }
            return default;
        }
        public bool SetData < T > (string key, T value, DateTimeOffset expirationTime) {
            TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            var isSet = _db.StringSet(key, JsonConvert.SerializeObject(value), expiryTime);
            return isSet;
        }
        public object RemoveData(string key) {
            bool _isKeyExist = _db.KeyExists(key);
            if (_isKeyExist == true) {
                return _db.KeyDelete(key);
            }
            return false;
        }
    }    
}