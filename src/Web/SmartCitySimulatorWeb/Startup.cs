using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using EventProcessorHost;

[assembly: OwinStartup(typeof(SmartCitySimulatorWeb.Startup))]

namespace SmartCitySimulatorWeb
{
    public class Startup
    {
        public static EventReceiver r;

        public void Configuration(IAppBuilder app)
        {
            // init SignalR
            app.MapSignalR();

            //Start receiving Event Hub events
            r = new EventReceiver();
            r.Start();
        }
    }
}
