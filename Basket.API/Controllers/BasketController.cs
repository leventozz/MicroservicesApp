using AutoMapper;
using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using EventBusRabbitMQ.Common;
using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Producer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly IMapper _mapper;
        private readonly EventBusRabbitMQProducer _eventBus;
        public BasketController(IBasketRepository repository, IMapper mapper, EventBusRabbitMQProducer eventBus)
        {
            _repository = repository;
            _mapper = mapper;
            _eventBus = eventBus;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BasketCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BasketCart>> GetBasket(string username)
        {
            var basket = await _repository.GetBasket(username);
            return Ok(basket ?? new BasketCart(username));
        }

        [HttpPost]
        [ProducesResponseType(typeof(BasketCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BasketCart>> UpdateBasket([FromBody]BasketCart basket)
        {
            return Ok( await _repository.UpdateBasket(basket));
        }

        [HttpDelete("{username}")]
        [ProducesResponseType(typeof(void),(int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string username)
        {
            return Ok(await _repository.DeleteBasket(username));
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            var basket = await _repository.GetBasket(basketCheckout.Username);

            if (basket == null)
                return BadRequest();

            var basketRemoved = await _repository.DeleteBasket(basket.Username);

            if (!basketRemoved)
                return BadRequest();

            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.RequestId = Guid.NewGuid();
            eventMessage.TotalPrice = basket.TotalPrice;

            try
            {
                _eventBus.PublishBasketCheckout(EventBusConstants.BasketCheckoutQueue, eventMessage);
            }
            catch (Exception)
            {
                //log logic
                throw;
            }

            return Accepted();
        }
    }
}
