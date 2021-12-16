using GeekBurger.StoreCatalog.Contract.Request;
using GeekBurger.StoreCatalog.ServiceBus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace GeekBurger.StoreCatalog.Controllers
{
    [ApiController]
    [Route("api/store")]
    public class StoreController : Controller
    {
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

            return Ok(new RequestStore(){ StoreName = storeName, Ready = true });
        }

        [HttpGet("GetMessage")]
        public IActionResult GetMessage()
        {

          
            var connectionBus = _configuration["ServiceBusConnectionString"];
            var config = new QueueConfigurationEngineServiceBus
            {
                ConnectionBus = connectionBus,
                QueueName = "store_dados",
                TopicName = ""
            };
            var delQueue = new QueueDelegateBus<string>
            {
                DoWork = ProcessarMessagensServiceBus,
                OnError = TratarErrosServiceBus
            };

           
            _serviceBusEngine.PublishMessage(config, JsonSerializer.Serialize(new RequestStore() { StoreName = "Teste pub", Ready = true }));

            _serviceBusEngine.SubscribeMessage(delQueue, config);
            return Ok();
        }

        [NonAction]
        public void ProcessarMessagensServiceBus(string data)
        {
            //Faz alguma coisa com a mensagem consumida do bus
            if (data != null) 
            {
            
            }
        }
        [NonAction]
        public void TratarErrosServiceBus(string ex) 
        {
            if (ex != null) { }
        }
    }
}
