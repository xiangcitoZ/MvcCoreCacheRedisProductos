using StackExchange.Redis;

namespace MvcCoreCacheRedisProductos.Helpers
{
    public static class HelperCacheMultiplexer
    {
        private static Lazy<ConnectionMultiplexer> CreateConnection =
            new Lazy<ConnectionMultiplexer>(() =>
            {
                string cnn = "bbddproductosredisxzx.redis.cache.windows.net:6380,password=ZkCEHTYLi5lukPGBM2eDv6edDQqxf5DGYAzCaKEkOO4=,ssl=True,abortConnect=False";
                return ConnectionMultiplexer.Connect(cnn);
            });

        public static ConnectionMultiplexer GetConnection
        {
            get { 
                return CreateConnection.Value;
            }
        }

    }
}
