using Azure.Messaging.ServiceBus;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog.ServiceBus
{
    public class ServiceBusEngine : IServiceBusEngine
    {
        static IQueueClient _queueClient;
        static ServiceBusClient _serviceBusClient;
        static ITopicClient _topicClient;
        public IQueueClient AbrirConexao(string connectionString, string queueName) => new QueueClient(connectionString, queueName);
        public ITopicClient AbrirConTopic(string con, string top) => new TopicClient(con, top);
        public async Task SubscribeMessage<T>(QueueDelegateBus<T> rpc, QueueConfigurationEngineServiceBus config)
        {
            if (_serviceBusClient == null)
                _serviceBusClient = new ServiceBusClient(config.ConnectionBus);
            ServiceBusReceiver receiver;
            if (config.TopicName != null)
            {
                try
                {
                    await using var client = new ServiceBusClient(config.ConnectionBus);
                    receiver = client.CreateReceiver(config.TopicName, config.Subscripton);
                    while (true)
                    {
                        try
                        {
                            IReadOnlyList<ServiceBusReceivedMessage> messages = await receiver.ReceiveMessagesAsync(maxMessages: 100);
                            if (messages.Any())
                            {
                                foreach (ServiceBusReceivedMessage message in messages)
                                {
                                    try
                                    {
                                        rpc.DoWork(Encoding.UTF8.GetString(message.Body));
                                        await receiver.CompleteMessageAsync(message);
                                    }
                                    catch (Exception)
                                    {
                                       await receiver.AbandonMessageAsync(message);                                       
                                    }                                  
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());                          
                        }
                    }
                }
                finally
                {                  
                    await _serviceBusClient.DisposeAsync();
                }
            }
            else
            {
                receiver = _serviceBusClient.CreateReceiver(config.QueueName);
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
        }

        public async Task PublishMessage<T>(QueueConfigurationEngineServiceBus config, T payload)
        {
            try
            {
                if (config.TopicName != null && config.Subscripton != null)
                {
                    if (_topicClient == null)
                        _topicClient = AbrirConTopic(config.ConnectionBus, config.TopicName);
                    await _topicClient.SendAsync(new Message(Encoding.UTF8.GetBytes(payload.ToString())));
                    await _topicClient.CloseAsync();
                    _topicClient = null;
                }
                else
                {

                    if (_queueClient == null)
                        _queueClient = AbrirConexao(config.ConnectionBus, config.QueueName);

                    await _queueClient.SendAsync(new Message(Encoding.UTF8.GetBytes(payload.ToString())));
                    await _queueClient.CloseAsync();
                    _queueClient = null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }      
    }
}
