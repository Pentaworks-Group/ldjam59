using GameFrame.Core.Math;

namespace Assets.Scripts.Core.Definitions
{
    public class PlanetDefinition : SpaceObjectDefinition
    {
        public PlanetLayerDefinition PlanetLayer { get; set; }
        public PlanetLayerDefinition SurfaceLayer { get; set; }
        public PlanetLayerDefinition CloudLayer { get; set; }
        public Vector2 Axis { get; set; }
    }
}
