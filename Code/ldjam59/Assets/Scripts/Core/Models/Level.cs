using System;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Models
{
    public class Level
    {
        public String Reference { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        // string Icon { get; set; }
        public string Seed { get; set; }

        public List<Planet> Planets { get; set; }

        public SimpleSpaceObject Source { get; set; }

        public SimpleSpaceObject Target { get; set; }

        public SimpleSpaceObject Signal { get; set; }

        //public List<EventDefinitions> Events { get; set; }

        public List<Signal> ActiveSignals { get; set; }
        public LevelScore Score { get; set; }
        public Double TimeElapsed { get; set; } = 0.0;
        public Int32 SignalsSend { get; set; } = 0;
        public Single SignalVelocityFactor { get; set; }
    }
}
