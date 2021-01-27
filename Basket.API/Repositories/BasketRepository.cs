using Basket.API.Data.Interfaces;
using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IBasketContext _basketContext;

        public BasketRepository(IBasketContext basketContext)
        {
            _basketContext = basketContext;
        }

        public async Task<bool> DeleteBasket(string username)
        {
            return await _basketContext.Redis.KeyDeleteAsync(username);
        }

        public async Task<BasketCart> GetBasket(string username)
        {
            var basket = await _basketContext.Redis.StringGetAsync(username);

            if (basket.IsNullOrEmpty)
                return null;

            return JsonConvert.DeserializeObject<BasketCart>(basket);
        }

        public async Task<BasketCart> UpdateBasket(BasketCart basket)
        {
            var result = await _basketContext.Redis.StringSetAsync(basket.Username, JsonConvert.SerializeObject(basket));

            if (!result)
                return null;

            return await GetBasket(basket.Username);
        }
    }
}
