

using Assets.Scripts.Core.Definitions;
using Assets.Scripts.Core.Models;
using System.Collections.Generic;

namespace Assets.Scripts.Core
{
    public class LevelConverter
    {

        public Level Convert(LevelDefinition levelDefinition)
        {

            var convertedLevel = new Level()
            {
                Name = levelDefinition.Name,
                Reference = levelDefinition.Reference,
                Description = levelDefinition.Description,
                Seed = levelDefinition.Seed,
            };

            if (levelDefinition.Planets?.Count > 0)
            {
                foreach (var planetDef in levelDefinition.Planets)
                {
                    convertedLevel.Planets.Add(ConvertPlanet(planetDef));
                }
            }

            convertedLevel.Source = ConvertSimpleSpaceObject(levelDefinition.Source);
            convertedLevel.Target = ConvertSimpleSpaceObject(levelDefinition.Target);
            convertedLevel.Signal = ConvertSimpleSpaceObject(levelDefinition.Signal);

            return convertedLevel;
        }

        private TSpaceObject ConvertSpaceObject<TSpaceObject, TSpaceObjectDefinition>(TSpaceObjectDefinition spaceObjectDefinition) where TSpaceObject : SpaceObject, new()
                                                                                                                                    where TSpaceObjectDefinition : SpaceObjectDefinition
        {
            return new TSpaceObject()
            {
                Name = spaceObjectDefinition.Name,
                Gravity = spaceObjectDefinition.Gravity,
                Id = spaceObjectDefinition.Id,
                Position = spaceObjectDefinition.Position,
                Size = spaceObjectDefinition.Size,
                Speed = spaceObjectDefinition.Speed,
            };
        }


        private SimpleSpaceObject ConvertSimpleSpaceObject(SimpleSpaceObjectDefinition source)
        {

            var simpleO = ConvertSpaceObject<SimpleSpaceObject, SimpleSpaceObjectDefinition>(source);
            simpleO.Sprite = source.Sprite;
            return simpleO;
        }



        private Planet ConvertPlanet(PlanetDefinition planetDef)
        {
            var convertedPlanet = ConvertSpaceObject<Planet, PlanetDefinition>(planetDef);

            convertedPlanet.Axis = planetDef.Axis;

            if (planetDef.Layers?.Count > 0)
            {
                ConvertLayers(planetDef, convertedPlanet);
            }

            return convertedPlanet;
        }

        private static void ConvertLayers(PlanetDefinition planetDef, Planet convertedPlanet)
        {
            convertedPlanet.Layers = new List<PlanetLayer>();
            foreach (var layerDef in planetDef.Layers)
            {
                var convertedLayer = new PlanetLayer()
                {
                    Color = layerDef.Color,
                    RotationSpeed = layerDef.RotationSpeed,
                    Texture = layerDef.Texture,
                };
                convertedPlanet.Layers.Add(convertedLayer);
            }
        }
    }
}
