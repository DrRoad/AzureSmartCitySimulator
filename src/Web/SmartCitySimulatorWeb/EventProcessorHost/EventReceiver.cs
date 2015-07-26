using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventProcessorHost
{
    public class EventReceiver
    {

        public void Start()
        {
            //string eventHubConnectionString = "{event hub connection string}";
            //string eventHubName = "{event hub name}";
            //string storageAccountName = "{storage account name}";
            //string storageAccountKey = "{storage account key}";
            //string consumerGroupName = "{consumer group name}";
            //string storageConnectionString = "{connection string to storage}";

            string eventHubConnectionString = AppConfig.eventHubConnectionString;
            string eventHubName = AppConfig.eventHubName;
            string storageAccountName = AppConfig.storageAccountName;
            string storageAccountKey = AppConfig.storageAccountKey;
            string consumerGroupName = AppConfig.consumerGroupName;
            string storageConnectionString = AppConfig.storageConnectionString;

            //string eventHubConnectionString = "Endpoint=sb://smartcitysimulator-ns.servicebus.windows.net/;SharedAccessKeyName=ReceiverRule;SharedAccessKey=O2b7yoNGFzyPzS7IXEEdLEkYG5TdNDpfRdp7iuKmvrE=";
            //string eventHubName = "smartcitysimulatoreventhub";
            //string storageAccountName = "smartcitysimulator";
            //string storageAccountKey = "PdMiEbkQVxhwhqGSSiimYfuTjKXaoBlVjmsWfScyMyMqHjQNObERU3kDpj30X7ndIEOlFmJ3cgaCmg9pOrTvSg==";
            //string consumerGroupName = "receiverconsole";
            //string storageConnectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}",
            //    storageAccountName, storageAccountKey);

            string eventProcessorHostName = Guid.NewGuid().ToString();
            Microsoft.ServiceBus.Messaging.EventProcessorHost eventProcessorHost = 
                new Microsoft.ServiceBus.Messaging.EventProcessorHost(
                                                                                                                eventProcessorHostName,
                                                                                                                eventHubName,
                                                                                                                consumerGroupName,
                                                                                                                eventHubConnectionString,
                                                                                                                storageConnectionString);

            //Console.WriteLine("Registering EventProcessor...");
            eventProcessorHost.RegisterEventProcessorAsync<EventProcessor>().Wait();
        }
    }
}