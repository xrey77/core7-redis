using StackExchange.Redis;
using System;


namespace core7_redis.Cache
{
   public class ConnectionHelper {



        static ConnectionHelper() {
            
            ConnectionHelper.lazyConnection = new Lazy < ConnectionMultiplexer > (() => {
                // Console.WriteLine(ConfigurationManager.AppSetting["RedisURL"]);
                return ConnectionMultiplexer.Connect(ConfigurationManager.AppSetting["RedisURL"]);
            });
        }
        private static Lazy < ConnectionMultiplexer > lazyConnection;
        public static ConnectionMultiplexer Connection {
            get {
                return lazyConnection.Value;
            }
        }
    }    
    
}