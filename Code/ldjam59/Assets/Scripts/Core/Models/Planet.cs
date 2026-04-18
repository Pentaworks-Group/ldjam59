using GameFrame.Core.Math;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Models
{
    public class Planet : SpaceObject
    {        
        public List<PlanetLayer> Layers { get; set; }
        public Vector2 Axis { get; set; }
    }
}
