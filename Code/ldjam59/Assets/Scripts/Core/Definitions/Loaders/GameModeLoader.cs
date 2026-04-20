using System.Collections.Generic;

using GameFrame.Core.Definitions.Loaders;

namespace Assets.Scripts.Core.Definitions.Loaders
{
    public class GameModeLoader : BaseLoader<GameMode>
    {

        public GameModeLoader(DefinitionCache<GameMode> gameModeCache) : base(gameModeCache)
        {

        }

        protected override void OnDefinitionsLoaded(List<GameMode> gameModeDefinitions)
        {
            _ = new GameMode() { IsReferenced = true };

            if (gameModeDefinitions?.Count > 0)
            {
                foreach (var loadedGameMode in gameModeDefinitions)
                {
                    var newGameMode = new GameMode()
                    {
                        IsDefault = loadedGameMode.IsDefault,
                        Reference = loadedGameMode.Reference,
                        Name = loadedGameMode.Name,
                        Levels = new List<LevelDefinition>(),
                        Audio = new AudioDefinition()
                    };

                    if (loadedGameMode.Audio != default)
                    {
                        if (loadedGameMode.Audio.IsTrackingObjects.HasValue)
                        {
                            newGameMode.Audio.IsTrackingObjects = loadedGameMode.Audio.IsTrackingObjects.Value;
                        }

                        if (loadedGameMode.Audio.IsConnectionLossEnabled.HasValue)
                        {
                            newGameMode.Audio.IsConnectionLossEnabled = loadedGameMode.Audio.IsConnectionLossEnabled.Value;
                        }
                    }

                    if (loadedGameMode.Levels != default)
                    {
                        CheckLevels(loadedGameMode.Levels, newGameMode.Levels);
                    }

                    targetCache[loadedGameMode.Reference] = newGameMode;
                }
            }
        }
        private void CheckLevels(List<LevelDefinition> loadedItems, List<LevelDefinition> targetItems)
        {
            if (loadedItems?.Count > 0)
            {
                foreach (var loadedItem in loadedItems)
                {
                    var targetLevel = new LevelDefinition()
                    {
                        Reference = loadedItem.Reference,
                        Name = loadedItem.Name,
                        Description = loadedItem.Description,
                        Planets = new List<PlanetDefinition>(),
                        SignalVelocityFactor = loadedItem.SignalVelocityFactor,
                    };

                    CheckPlanets(loadedItem.Planets, targetLevel.Planets);

                    targetLevel.Source = loadedItem.Source;
                    targetLevel.Target = loadedItem.Target;
                    targetLevel.Signal = loadedItem.Signal;

                    targetItems.Add(targetLevel);
                }
            }
        }

        private void CheckPlanets(List<PlanetDefinition> loadedPlanets, List<PlanetDefinition> targetPlanets)
        {
            if (loadedPlanets?.Count > 0)
            {
                targetPlanets.AddRange(loadedPlanets);
            }
        }
    }
}
