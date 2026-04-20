using System;
using System.Collections.Generic;

using GameFrame.Core.Definitions;

namespace Assets.Scripts.Core.Definitions
{
    public class LevelDefinition : BaseDefinition
    {
        public string Name { get; set; }
        public string Description { get; set; }
        // string Icon { get; set; }
        public string Seed { get; set; }
        public Single? SignalVelocityFactor { get; set; }

        public List<PlanetDefinition> Planets { get; set; }

        public SimpleSpaceObjectDefinition Source { get; set; }

        public SimpleSpaceObjectDefinition Target { get; set; }

        public SimpleSpaceObjectDefinition Signal { get; set; }

        //public List<EventDefinitions> Events { get; set; }
    }
}
