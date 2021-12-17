using Azure.Messaging.ServiceBus;
using System;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog.ServiceBus
{
    public interface IServiceBusEngine
    {
        Task SubscribeMessage<T>(QueueDelegateBus<T> rpc, QueueConfigurationEngineServiceBus config);
        Task PublishMessage<T>(QueueConfigurationEngineServiceBus config, T payload);
    }

    public sealed class QueueDelegateBus<T>
    {
        public QueueDelegateBus()
        {
            DoWork = NonNullableInitialization<string>;
            OnError = NonNullableInitialization;
          
        }
        private void NonNullableInitialization<TType>(TType type)
        {

        }

        public Action<string> DoWork { get; set; }
        public Action<string> OnError { get; set; }        
    }


    public sealed class QueueConfigurationEngineServiceBus
    {
        public string ConnectionBus { get; set; }
        public string QueueName { get; set; }
        public string TopicName { get; set; }

        public string Subscripton { get; set; }
    }

}
