using System.Collections.Generic;

namespace Assets.Scripts.Core.Definitions
{
    public class LevelDefinition
    {
        public string Name { get; set; }
        public string Description { get; set; }
        // string Icon { get; set; }

        public List<PlanetDefinition> Planets { get; set; }

        public SpaceObjectDefinition Source { get; set; }

        public SpaceObjectDefinition Target { get; set; }

        public SpaceObjectDefinition Signal { get; set; }

        //public List<EventDefinitions> Events { get; set; }
    }
}
