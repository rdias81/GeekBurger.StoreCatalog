using Azure.Messaging.ServiceBus;
using GeekBurger.StoreCatalog.Contract.Request;
using GeekBurger.StoreCatalog.ServiceBus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using System;
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
        public StoreController(IConfiguration configuration, IServiceBusEngine serviceBusEngine)
        {
            _configuration = configuration;
            _serviceBusEngine = serviceBusEngine;
        }
        [HttpGet]
        [Route("{storeName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult GetStoreStatus(string storeName)
        {
            if (storeName == "testeErro")
                return StatusCode(503);

            return Ok(new RequestStore() { StoreName = storeName, Ready = true });
        }

        [HttpGet("GetMessage")]
        public async Task<IActionResult> GetMessage()
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
            await _serviceBusEngine.PublishMessage(config, JsonSerializer.Serialize(new RequestStore() { StoreName = "Teste pub GeekBurger.StoreCatalog", Ready = true }));
            await _serviceBusEngine.SubscribeMessage(delQueue, config);
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
