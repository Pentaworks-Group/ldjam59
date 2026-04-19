using Assets.Scripts.Core.Definitions;
using Assets.Scripts.Core.Definitions.Loaders;
using Assets.Scripts.Core.Persistence;
using Assets.Scripts.Scenes.Game;
using GameFrame.Core.Definitions.Loaders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Core
{
    public class Game : GameFrame.Core.SaveableGame<GameState, PlayerOptions, SavedGamePreview>
    {
        private readonly DefinitionCache<Definitions.GameMode> gameModeCache = new DefinitionCache<Definitions.GameMode>();

        private GameMode selectedGameMode;


        public UnityEvent OnModelUpdate = new UnityEvent();

        public IList<Definitions.GameMode> GetAvailableGameModes()
        {
            return this.gameModeCache.Values.ToList();
        }

        public void Start(GameMode gameMode)
        {
            this.selectedGameMode = gameMode;
            Start();
        }

        protected override GameState InitializeGameState()
        {
            var gameMode = selectedGameMode;
                        
            if (gameMode == default)
            {
                gameMode = gameModeCache.Values.FirstOrDefault();
            }

            if (gameMode == default)
            {
                throw new Exception("What happend? No GameMode!!");
            }

            var gameStateConverter = new GameStateConverter(gameMode);

            var gameState = gameStateConverter.Convert();

            return gameState;
        }

        protected override PlayerOptions InitializePlayerOptions()
        {
            return new PlayerOptions()
            {
                EffectsVolume = 0.9f,
                AmbienceVolume = 0.1f,
                BackgroundVolume = 0.3f,
            };
        }

        protected override void RegisterScenes()
        {
            RegisterScenes(Constants.Scenes.GetAll());
        }

        protected override IEnumerator LoadDefintions()
        {
            yield return new GameModeLoader(this.gameModeCache).LoadDefinitions("GameModes.json");
            Debug.Log("loaded definitions");
        }
    }
}
