using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.ServiceBus.Messaging;
using System.Diagnostics;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using SmartCitySimulatorWeb;

//http://www.asp.net/signalr/overview/getting-started/tutorial-server-broadcast-with-signalr

namespace EventProcessorHost
{
    class EventProcessor : IEventProcessor
    {
        IHubContext hubContext;

        Stopwatch checkpointStopWatch;

        async Task IEventProcessor.CloseAsync(PartitionContext context, CloseReason reason)
        {
            if (reason == CloseReason.Shutdown)
            {
                await context.CheckpointAsync();
            }
        }

        Task IEventProcessor.OpenAsync(PartitionContext context)
        {
            this.checkpointStopWatch = new Stopwatch();
            this.checkpointStopWatch.Start();
            return Task.FromResult<object>(null);
        }

        // This is fired each time there is a new event in the event hub
        async Task IEventProcessor.ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            // Get the SignalR hub
            if(hubContext == null)
                hubContext = GlobalHost.ConnectionManager.GetHubContext
                                        <SmartCitySimulatorWeb.SmartCitySimulatorHub>();

            foreach (EventData eventMessage in messages)
            {
                // Get the json event data from the event hub
                string eventJsonString = Encoding.UTF8.GetString(eventMessage.GetBytes());
                SmartCityEvent smartCityEvent = JsonConvert.DeserializeObject<SmartCityEvent>(eventJsonString);

                // pass the smartCityEvent to all the clients
                hubContext.Clients.All.updateCharts(smartCityEvent);
            }

            //Call checkpoint every 10 seconds, so that worker can resume processing from 10 seconds back if it restarts.
            if (this.checkpointStopWatch.Elapsed > TimeSpan.FromSeconds(10))
            {
                await context.CheckpointAsync();
                this.checkpointStopWatch.Restart();
            }
        }
    }
}
