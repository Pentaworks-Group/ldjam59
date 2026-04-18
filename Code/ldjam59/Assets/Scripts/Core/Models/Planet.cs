using GameFrame.Core.Math;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Models
{
    public class Planet : SpaceObject
    {        
        public List<PlanetLayer> Layers { get; set; }
        public Vector3 Axis { get; set; }
    }
}
