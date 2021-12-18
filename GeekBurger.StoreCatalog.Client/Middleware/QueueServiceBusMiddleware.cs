using GeekBurger.StoreCatalog.Client.ServiceBus;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog.Client.Middleware
{
    public class QueueServiceBusMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _appSettings;
        private readonly IServiceBusEngine _serviceBusEngine;

        public QueueServiceBusMiddleware(RequestDelegate next, IConfiguration appSettings, IServiceBusEngine serviceBusEngine)
        {
            _next = next;
            _appSettings = appSettings;
            _serviceBusEngine = serviceBusEngine;
        }
        public async Task Invoke(HttpContext context)
        {
           // var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            ListenerQueueProductChanged();
            ListenerQueueProductionAreaChanged();

            await _next(context);
        }


        public async void ListenerQueueProductionAreaChanged()
        {
            var connectionBus = _appSettings["ServiceBusConnectionString"];
            var config = new QueueConfigurationEngineServiceBus
            {
                ConnectionBus = connectionBus,
                QueueName = null,
                TopicName = "productionareachanged",
                Subscripton = "store-catalog"
            };
            var delQueue = new QueueDelegateBus<string>
            {
                DoWork = ProcessarMessagensProductionAreaChanged,
                OnError = TratarErrosProductionAreaChanged
            };
            // await _serviceBusEngine.PublishMessage(config, JsonSerializer.Serialize(new StoreCatalogReady() { StoreName = "Paulista", Ready = true }));
            await _serviceBusEngine.SubscribeMessage(delQueue, config);
        }
        private void ProcessarMessagensProductionAreaChanged(string obj)
        {
            if (obj != null)
            {

            }
        }
        private void TratarErrosProductionAreaChanged(string obj)
        {
            throw new System.NotImplementedException();
        }

        public async void ListenerQueueProductChanged()
        {
            var connectionBus = _appSettings["ServiceBusConnectionString"];
            var config = new QueueConfigurationEngineServiceBus
            {
                ConnectionBus = connectionBus,
                QueueName = null,
                TopicName = "productchanged",
                Subscripton = "store-catalog"
            };
            var delQueue = new QueueDelegateBus<string>
            {
                DoWork = ProcessarMessagensProductChanged,
                OnError = TratarErrosProductChanged
            };
            // await _serviceBusEngine.PublishMessage(config, JsonSerializer.Serialize(new StoreCatalogReady() { StoreName = "Paulista", Ready = true }));
            await _serviceBusEngine.SubscribeMessage(delQueue, config);

        }

        private void TratarErrosProductChanged(string obj)
        {
            throw new System.NotImplementedException();
        }

        private void ProcessarMessagensProductChanged(string obj)
        {
            if (obj != null) 
            {
            
            }
        }
    }
}
