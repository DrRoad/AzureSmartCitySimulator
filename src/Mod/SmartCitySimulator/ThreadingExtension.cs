using ICities;
//using UnityEngine;
using ColossalFramework;
//using ColossalFramework.Plugins;

using System.Net;
using System;
//using System.IO;
//using System.IO.Pipes;
//using System.Text;
//using System.Security.Principal;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;
//using System.Threading;

namespace SmartCitySimulator
{

    public class ThreadingExtension: IThreadingExtension
    {
        Stopwatch stopWatch;
        string cityName; // The name of your city
        DateTime gameTime; // The date and time in your city

        SimulationManager simulationManager;
        int eventFrequency = 1; //number of seconds between sending events to Azure

        //Thread: Main
        public void OnCreated(IThreading threading)
        {
            Debug.Log("MyIThreadingExtension Created");

            simulationManager = Singleton<SimulationManager>.instance;
            

            this.stopWatch = new Stopwatch();
            this.stopWatch.Start();

            //SendEvent();

            Debug.Log("MyIThreadingExtension Created Complete");

        }

        private void SendEvent()
        {
            string serviceBusNamespace = AppConfig.serviceBusNamespace;
            string eventHubName = AppConfig.eventHubName;
            string publisherName = AppConfig.publisherName;
            string sas = AppConfig.sas;

            Uri uri = new Uri("https://" + serviceBusNamespace +
                                        ".servicebus.windows.net/" + eventHubName +
                                        "/publishers/" + publisherName + "/messages");

            WebClient wc = new WebClient();
            //Next 2 lines are a HACK to work around a MONO bug when posting
            System.Net.ServicePointManager.ServerCertificateValidationCallback += (o, certificate, chain, errors) => true;
            System.Security.Cryptography.AesCryptoServiceProvider b = new System.Security.Cryptography.AesCryptoServiceProvider();

            wc.Headers[HttpRequestHeader.ContentType] = "application/atom+xml;type=entry;charset=utf-8";
            wc.Headers[HttpRequestHeader.Authorization] = sas;
            //HACK: 
            System.Random rnd = new System.Random();
            string temp = rnd.Next(1, 100).ToString();

            int ParkedCount;
            int VehicleCount;
            getVehicleCounts(out ParkedCount, out VehicleCount);

            decimal income;
            decimal expenses;
            getEconomyData(out income, out expenses);

            SmartCityEvent scEvent = new SmartCityEvent
            {
                gameTime = this.gameTime,
                cityName = cityName,
                parkedCount = ParkedCount,
                vehicleCount = VehicleCount,
                income = income,
                expenses = expenses
            };

            //UnityEngine.Debug.Log("Starting to send Azure event");
            try {
                string scEventJson = serializeJson(scEvent);
                //string scEventJson = "test"; 
                Debug.Log(scEventJson);
                //string postString = "{id:\"" + cityName + "\",parkedCount:\"" + ParkedCount + "\",vehicleCount:\"" + VehicleCount + "\"}";
                //wc.UploadString(uri, "POST", "{id:\"" + cityName + "\",temp:\"" + temp + "\"}");
                wc.UploadString(uri, "POST", scEventJson);
                Debug.Log("POST: " + scEventJson);
            }
            catch(Exception ex )
            {
                UnityEngine.Debug.LogError(ex.Message);
            }
            //UnityEngine.Debug.Log("Completed send to Azure event");
        }

        private string serializeJson(SmartCityEvent scEvent)
        {
            //++++
            //HACK: NOT WORKING. Need to do this manually for now
            //++++

            //need to use the DataContractJsonSerializer becuase NewtonSoft Json does not work in Mono

            //MemoryStream scEventStream = new MemoryStream();
            //DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(SmartCityEvent));
            //ser.WriteObject(scEventStream, scEvent);

            //string jsonString = Encoding.Default.GetString((scEventStream.ToArray()));
            //string postString = "{id:\"" + cityName + "\",parkedCount:\"" + ParkedCount + "\",vehicleCount:\"" + VehicleCount + "\"}";

            string jsonString = String.Format(@"{{" +
                                @"""gameTime"":""{0}""," +
                                @"""cityName"":""{1}""," +
                                @"""parkedCount"":""{2}""," +
                                @"""vehicleCount"":""{3}""," +
                                @"""income"":""{4}""," +
                                @"""expenses"":""{5}""" +
                                @"}}",
                                scEvent.gameTime.ToString("MM/dd/yyyy hh:mm:ss.fff tt"), // 0 output date and time with milliseconds
                                scEvent.cityName, //1
                                scEvent.parkedCount.ToString(),  //2
                                scEvent.vehicleCount.ToString(), //3
                                scEvent.income.ToString(), //4
                                scEvent.expenses.ToString()); //5

            return jsonString;
        }

        public void getVehicleCounts(out int ParkedCount, out int VehicleCount)
        {
            //Get the Vechicle Manager
            VehicleManager vehicleManager = Singleton<VehicleManager>.instance;

            ParkedCount = vehicleManager.m_parkedCount;
            VehicleCount = vehicleManager.m_vehicleCount; 
         }

        public void getEconomyData(out decimal incomeAmount, out decimal expensesAmount)
        {
            EconomyManager economyManager = Singleton<EconomyManager>.instance;
            long income;
            long expenses;
            economyManager.GetIncomeAndExpenses(new ItemClass(), out income, out expenses);

            incomeAmount = Math.Round(((decimal)income / 100), 2);
            expensesAmount = Math.Round(((decimal)expenses / 100), 2);

        }

        //Thread: Main
        public void OnReleased()
		{
		
		}
		//Thread: Main
		public void OnUpdate(float realTimeDelta, float simulationTimeDelta)
		{
		
		}
		//Thread: Simulation
		public void OnBeforeSimulationTick()
		{
            //Debug.Log("Stopwatch > " + stopWatch.Elapsed.ToString());
            //fire event every n seconds
            if (this.stopWatch.Elapsed > TimeSpan.FromSeconds(eventFrequency))
            {
                //Debug.Log("MyIThreadingExtension OnBeforeSimulationTick");
                cityName = simulationManager.m_metaData.m_CityName;
                gameTime = simulationManager.m_currentGameTime;
                //Debug.Log("Game Time: " + simulationManager.m_currentGameTime.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));

                SendEvent();

                //Debug.Log("MyIThreadingExtension OnBeforeSimulationTick Complete");

                this.stopWatch.Reset();
                this.stopWatch.Start();
            }
        }

		//Thread: Simulation
		public void OnBeforeSimulationFrame()
		{
		
		}
		//Thread: Simulation
		public void OnAfterSimulationFrame()
		{
		
		}
		//Thread: Simulation
		public void OnAfterSimulationTick()
		{
		
		}

	 }
}
