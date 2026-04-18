using GameFrame.Core.Math;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Definitions
{
    public class PlanetDefinition : SpaceObjectDefinition
    {        
        public List<PlanetLayerDefinition> Layers { get; set; }
        public Vector2 Axis { get; set; }
    }
}
