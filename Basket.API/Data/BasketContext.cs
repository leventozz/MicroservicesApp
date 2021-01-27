using Basket.API.Data.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Data
{
    public class BasketContext : IBasketContext
    {
        private readonly ConnectionMultiplexer _redisConn;

        public BasketContext(ConnectionMultiplexer redisConn)
        {
            _redisConn = redisConn;
            Redis = _redisConn.GetDatabase();
        }

        public IDatabase Redis { get; }
    }
}
