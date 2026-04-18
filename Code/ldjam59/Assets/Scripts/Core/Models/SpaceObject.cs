using GameFrame.Core.Definitions;
using GameFrame.Core.Math;
using System.Collections.Generic;

namespace Assets.Scripts.Core.Models
{
    public class SpaceObject
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public float? Size { get; set; }
        public float? Gravity { get; set; }
        public Vector2? Position { get; set; }
        public Vector2? Speed { get; set; }
        public List<Orbiter> Orbiters { get; set; }
    }
}
