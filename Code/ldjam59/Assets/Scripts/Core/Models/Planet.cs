using GameFrame.Core.Math;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Models
{
    public class Planet : SpaceObject
    {        
        public PlanetLayer PlanetLayer { get; set; }
        public PlanetLayer SurfaceLayer { get; set; }
        public PlanetLayer CloudLayer { get; set; }

        public Vector2 Axis { get; set; }
    }
}
