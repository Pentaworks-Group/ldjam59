

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
                ActiveSignals = new List<Signal>(),
            };

            if (levelDefinition.Planets?.Count > 0)
            {
                foreach (var planetDef in levelDefinition.Planets)
                {
                    convertedLevel.Planets.Add(ConvertSpaceObject<Planet, PlanetDefinition>(planetDef, PlanetConversion));
                }
            }

            convertedLevel.Source = ConvertSpaceObject<SimpleSpaceObject, SimpleSpaceObjectDefinition>(levelDefinition.Source, SimpleObject);
            convertedLevel.Target = ConvertSpaceObject<SimpleSpaceObject, SimpleSpaceObjectDefinition>(levelDefinition.Target, SimpleObject);
            convertedLevel.Signal = ConvertSpaceObject<SimpleSpaceObject, SimpleSpaceObjectDefinition>(levelDefinition.Signal, SimpleObject);

            return convertedLevel;
        }

        private TModel ConvertSpaceObject<TModel, TDefinition>(TDefinition definition, Action<TDefinition, TModel> factory) where TModel : SpaceObject, new()
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
            model.Color = definition.Color;
        }

        private static void PlanetConversion(PlanetDefinition planetDef, Planet convertedPlanet)
        {
            convertedPlanet.Axis = planetDef.Axis;
            convertedPlanet.PlanetLayer = ConvertLayer(planetDef.PlanetLayer);
            convertedPlanet.SurfaceLayer = ConvertLayer(planetDef.SurfaceLayer);
            convertedPlanet.CloudLayer = ConvertLayer(planetDef.CloudLayer);
        }

        private static PlanetLayer ConvertLayer(PlanetLayerDefinition layerDef)
        {
            return new PlanetLayer()
            {
                Color = layerDef.Color,
                RotationSpeed = layerDef.RotationSpeed,
                Texture = layerDef.Texture,
            };
        }
    }
}
