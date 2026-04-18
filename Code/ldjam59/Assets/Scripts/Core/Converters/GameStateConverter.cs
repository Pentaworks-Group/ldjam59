using System;

using Assets.Scripts.Core.Definitions;
using Assets.Scripts.Core.Models;

namespace Assets.Scripts.Core
{
    public class GameStateConverter
    {
        private readonly GameMode mode;

        public GameStateConverter(GameMode gameMode)
        {
            this.mode = gameMode;
        }

        public GameState Convert()
        {

            var gameState = new GameState()
            {
                CreatedOn = DateTime.Now,
                CurrentScene = Constants.Scenes.GameName,
                Mode = mode,
            };

            if (this.mode.Levels?.Count > 0)
            {
                var firstLevel = this.mode.Levels[0];

                gameState.CurrentLevel = new LevelConverter().Convert(firstLevel);

            }
            else
            {
                throw new Exception("No Levels provided in GameMode!");
            }

            return gameState;
        }
    }
}
