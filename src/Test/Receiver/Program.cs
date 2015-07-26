using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.ServiceBus.Messaging;


//https://azure.microsoft.com/en-us/documentation/articles/service-bus-event-hubs-c-ephcs-getstarted/#receive-messages-with-eventprocessorhost

namespace Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            string eventHubConnectionString = AppConfig.eventHubConnectionString;
            string eventHubName = AppConfig.eventHubName;
            string storageAccountName = AppConfig.storageAccountName;
            string storageAccountKey = AppConfig.storageAccountKey;
            string consumerGroupName = AppConfig.consumerGroupName;
            string storageConnectionString = AppConfig.storageConnectionString;

            string eventProcessorHostName = Guid.NewGuid().ToString();
            EventProcessorHost eventProcessorHost = new EventProcessorHost(
                eventProcessorHostName, 
                eventHubName,
                consumerGroupName, 
                eventHubConnectionString, 
                storageConnectionString);

            Console.WriteLine("Registering EventProcessor...");
            eventProcessorHost.RegisterEventProcessorAsync<SimpleEventProcessor>().Wait();

            Console.WriteLine("Receiving. Press enter key to stop worker.");
            Console.ReadLine();
            eventProcessorHost.UnregisterEventProcessorAsync().Wait();
        }
    }
}
