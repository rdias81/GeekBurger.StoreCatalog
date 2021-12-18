using GeekBurger.StoreCatalog.Client.ServiceBus;
using GeekBurger.StoreCatalog.Contract;
using GeekBurger.StoreCatalog.DataCache;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog.Client.Middleware
{
    public class QueueServiceBusMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _appSettings;
        private readonly IServiceBusEngine _serviceBusEngine;
        private readonly IMemoryCache _memoryCache;

        private const string StoreReady = "store-ready-cache";
        private const string ProductChanged = "product-changed-cache";
        private const string ProductionAreaChanged = "production-area-changed-cache";

        public QueueServiceBusMiddleware(RequestDelegate next, IConfiguration appSettings, IServiceBusEngine serviceBusEngine, IMemoryCache memoryCache)
        {
            _next = next;
            _appSettings = appSettings;
            _serviceBusEngine = serviceBusEngine;
            _memoryCache = memoryCache;
        }
        public async Task Invoke(HttpContext context)
        {          
            
            PublishStoreReady();
            ListenerQueueProductChanged();
            ListenerQueueProductionAreaChanged();
            await _next(context);
        }
        private async void PublishStoreReady()
        {
            var storeReadyCache = _memoryCache.Get(StoreReady);
            if (storeReadyCache == null)
            {
                var nomeLoja = "Morumbi";
                ServiceBusEngine serviceBusEngine = new ServiceBusEngine();
                var connectionBus = _appSettings["ServiceBusConnectionString"];
                var config = new QueueConfigurationEngineServiceBus
                {
                    ConnectionBus = connectionBus,
                    QueueName = null,
                    TopicName = "storecatalogready",
                    Subscripton = "store-catalog"
                };
                var storeCatalog = new StoreCatalogReady() { StoreName = nomeLoja, Ready = true };
                await serviceBusEngine.PublishMessage(config, JsonSerializer.Serialize(storeCatalog));
                _memoryCache.Set(StoreReady, JsonSerializer.Serialize(storeCatalog));
            }
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
            await _serviceBusEngine.SubscribeMessage(delQueue, config);
        }
        private void ProcessarMessagensProductionAreaChanged(string obj)
        {
            if (obj != null)
            {
                var productArea = JsonSerializer.Deserialize<Production.Contract.Areas>(obj);
                //Logica abaixo pode ser substituida para salvar produto consumido da fila ProductionAreaChanged no banco de dados;
                var existProductArea = _memoryCache.Get(productArea.ProductionId);
                if (existProductArea != null)
                {
                    _memoryCache.Remove(productArea.ProductionId);
                    _memoryCache.Set(productArea.ProductionId, productArea);
                }
                else
                {
                    _memoryCache.Set(productArea.ProductionId, productArea);
                }
            }
        }
        private void TratarErrosProductionAreaChanged(string obj)
        {
            _memoryCache.Set(ProductionAreaChanged, obj);
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
            await _serviceBusEngine.SubscribeMessage(delQueue, config);
        }    
        private void ProcessarMessagensProductChanged(string obj)
        {
            if (obj != null)
            {
                var product = JsonSerializer.Deserialize<ResponseProduct>(obj);
                //Logica abaixo pode ser substituida para salvar produto consumido da fila ProductChanged no banco de dados;
                var existProduct = _memoryCache.Get(product.ProductId);
                if (existProduct != null)
                {
                    _memoryCache.Remove(product.ProductId);
                    _memoryCache.Set(product.ProductId, product);
                }
                else
                {
                    _memoryCache.Set(product.ProductId, product);
                }
            }
        }
        private void TratarErrosProductChanged(string obj)
        {
            _memoryCache.Set(ProductChanged, obj);
        }
    }

}
