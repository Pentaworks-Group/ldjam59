

using Assets.Scripts.Core.Definitions;
using Assets.Scripts.Core.Models;

using System;
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
                Planets = new List<Planet>(),
            };

            if (levelDefinition.Planets?.Count > 0)
            {
                foreach (var planetDef in levelDefinition.Planets)
                {
                    convertedLevel.Planets.Add(ConvertPlanet(planetDef));
                }
            }

            convertedLevel.Source = Test<SimpleSpaceObject, SimpleSpaceObjectDefinition>(levelDefinition.Source, SimpleObject);
            convertedLevel.Target = Test<SimpleSpaceObject, SimpleSpaceObjectDefinition>(levelDefinition.Source, SimpleObject);
            convertedLevel.Signal = Test<SimpleSpaceObject, SimpleSpaceObjectDefinition>(levelDefinition.Source, SimpleObject);

            return convertedLevel;
        }

        private TSpaceObject ConvertSpaceObject<TSpaceObject, TSpaceObjectDefinition>(TSpaceObjectDefinition spaceObjectDefinition) where TSpaceObject : SpaceObject, new()
                                                                                                                                    where TSpaceObjectDefinition : SpaceObjectDefinition
        {
            if (spaceObjectDefinition != default)
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

            return default;
        }

        private TModel Test<TModel, TDefinition>(TDefinition definition, Action<TDefinition, TModel> factory) where TModel : SpaceObject, new()
                                                                                                              where TDefinition : SpaceObjectDefinition
        {
            var result = default(TModel);

            if (definition != default)
            {
                result = new TModel()
                {
                    Name = definition.Name,
                    Gravity = definition.Gravity,
                    Id = definition.Id,
                    Position = definition.Position,
                    Size = definition.Size,
                    Speed = definition.Speed,
                };

                factory(definition, result);
            }

            return result;
        }

        private void SimpleObject(SimpleSpaceObjectDefinition definition, SimpleSpaceObject model)
        {
            model.Sprite = definition.Sprite;
        }

        private SimpleSpaceObject ConvertSimpleSpaceObject(SimpleSpaceObjectDefinition source)
        {
            var result = ConvertSpaceObject<SimpleSpaceObject, SimpleSpaceObjectDefinition>(source);

            result.Sprite = source.Sprite;

            return result;
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
