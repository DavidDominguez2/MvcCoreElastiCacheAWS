using StackExchange.Redis;

namespace MvcCoreElastiCacheAWS.Helpers {
    public class HelperCacheRedis {

        private static Lazy<ConnectionMultiplexer> CreateConnection = new Lazy<ConnectionMultiplexer>(() => {
            return ConnectionMultiplexer.Connect("cache-coches.s79zfv.ng.0001.use1.cache.amazonaws.com:6379");
        });

        public static ConnectionMultiplexer Connection {
            get { return CreateConnection.Value; }
        }
    }
}
