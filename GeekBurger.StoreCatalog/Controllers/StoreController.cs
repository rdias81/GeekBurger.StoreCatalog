using GeekBurger.StoreCatalog.Client.ServiceBus;
using GeekBurger.StoreCatalog.Contract;
using GeekBurger.StoreCatalog.Contract.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog.Controllers
{
    [ApiController]
    [Route("api/store")]
    public class StoreController : Controller
    {

       // const string QueueConnectionString = "Endpoint=sb://geekburgernet.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Z8l0qsaaDXT7gv5z0lUlmB1dH/ISKypGf3/OVFOIGsU=";
       // const string QueuePath = "ProductChanged2";
        private readonly IConfiguration _configuration;
        private readonly IServiceBusEngine _serviceBusEngine;
        private readonly IMemoryCache _cache;

        public StoreController(IConfiguration configuration, IServiceBusEngine serviceBusEngine, IMemoryCache cache)
        {
            _configuration = configuration;
            _serviceBusEngine = serviceBusEngine;
            _cache = cache;
        }

        [HttpGet]
        [Route("{storeName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult GetStoreStatus(string storeName)
        {
            var loja = JsonSerializer.Deserialize<StoreCatalogReady>(_cache.Get("store-ready-cache").ToString(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            if (loja == null || !loja.StoreName.Equals(storeName, StringComparison.InvariantCultureIgnoreCase))
                return StatusCode(503);

            return Ok(new RequestStore() { StoreName = loja.StoreName, Ready = loja.Ready });
        }

        [NonAction]
        //[HttpGet("PublishStoreCatalogReady")]
        public async Task<IActionResult> PublishStoreCatalogReady()
        {

            var connectionBus = _configuration["ServiceBusConnectionString"];
            var config = new QueueConfigurationEngineServiceBus
            {
                ConnectionBus = connectionBus,
                QueueName = null,
                TopicName = "storecatalogready",
                Subscripton = "store-catalog"
            };
            var delQueue = new QueueDelegateBus<string>
            {
                DoWork = ProcessarMessagensServiceBus,
                OnError = TratarErrosServiceBus
            };
           // await _serviceBusEngine.PublishMessage(config, JsonSerializer.Serialize(new StoreCatalogReady() { StoreName = "Paulista", Ready = true }));
           await _serviceBusEngine.SubscribeMessage(delQueue, config);
            return Ok();
        }

        [NonAction]
      //  [HttpGet("PublishUserWithLessOffer")]
        public async Task<IActionResult> PublishUserWithLessOffer()
        {

            var connectionBus = _configuration["ServiceBusConnectionString"];
            var config = new QueueConfigurationEngineServiceBus
            {
                ConnectionBus = connectionBus,
                QueueName = null,
                TopicName = "userwithlessoffer",
                Subscripton = "store-catalog"
            };
            //use configuracoes abaixo apenas para subscribe
            //var delQueue = new QueueDelegateBus<string>
            //{
            //    DoWork = ProcessarMessagensServiceBus,
            //    OnError = TratarErrosServiceBus
            //};
            await _serviceBusEngine.PublishMessage(config, JsonSerializer.Serialize(new UserWithLessOffer() { UserId = 1, Restrictions = new List<string>() { "soy", "diary", "peanut" } }));
          //  await _serviceBusEngine.SubscribeMessage(delQueue, config);
            return Ok();
        }



        [NonAction]
        public void ProcessarMessagensServiceBus(string data)
        {
            //Faz alguma coisa com a mensagem consumida do bus
            if (data != null)
            {
                Console.WriteLine(data);
            }
        }
        [NonAction]
        public void TratarErrosServiceBus(string ex)
        {
            if (ex != null) { }
        }

    }
}
