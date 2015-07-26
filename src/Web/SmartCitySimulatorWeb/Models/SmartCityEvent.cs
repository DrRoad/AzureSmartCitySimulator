using System;

namespace SmartCitySimulatorWeb
{
    public class SmartCityEvent
    {
        // City Data
        public DateTime gameTime { get; set; }
        public string cityName { get; set; }

        // Traffic Data
        public int parkedCount { get; set; }
        public int vehicleCount { get; set; }

        // Budget Data
        public decimal income { get; set; }
        public decimal expenses { get; set; }

    }
}
