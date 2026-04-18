using Assets.Scripts.Core.Models;

using GameFrame.Core.Definitions.Loaders;
using System.Collections.Generic;

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
                    };

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
                        Planets = new List<PlanetDefinition>()
                    };

                    CheckPlanets(loadedItem.Planets, targetLevel.Planets);

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
