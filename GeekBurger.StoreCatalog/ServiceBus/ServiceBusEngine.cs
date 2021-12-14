using Azure.Messaging.ServiceBus;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog.ServiceBus
{
    public class ServiceBusEngine : IServiceBusEngine
    {
        static IQueueClient _queueClient;
        static ServiceBusClient _serviceBusClient;

        public IQueueClient AbrirConexao(string connectionString, string queueName) => new QueueClient(connectionString, queueName);
        public async Task SubscribeMessage<T>(QueueDelegateBus<T> rpc, QueueConfigurationEngineServiceBus config)
        {
            if (_serviceBusClient.IsClosed)
                _serviceBusClient = new ServiceBusClient(config.ConnectionBus);
            ServiceBusReceiver receiver = _serviceBusClient.CreateReceiver(config.QueueName);
            try
            {
                ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync();
                rpc.DoWork(receivedMessage.Body.ToString());
                await receiver.CompleteMessageAsync(receivedMessage);
            }
            catch (Exception ex)
            {
                rpc.OnError(ex.Message);
            }
        }
        public async Task PublishMessage<T>(QueueConfigurationEngineServiceBus config, T payload)
        {

            if (_queueClient.IsClosedOrClosing)
                _queueClient = AbrirConexao(config.ConnectionBus, config.QueueName);

            await _queueClient.SendAsync(new Message(Encoding.UTF8.GetBytes(payload.ToString())));
            await _queueClient.CloseAsync();
        }




    }
}
