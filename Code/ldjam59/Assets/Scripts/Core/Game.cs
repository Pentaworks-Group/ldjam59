using Assets.Scripts.Core.Definitions;
using Assets.Scripts.Core.Definitions.Loaders;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Persistence;
using Assets.Scripts.Scenes.Game;
using GameFrame.Core;
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



        public override void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
            //Application.Quit(); 
#elif UNITY_STANDALONE
            Application.Quit();            
#endif
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

        private int GetCurrentLevelIndex()
        {
            var levels = Base.Core.Game.State.Mode.Levels;
            var currentLevel = Base.Core.Game.State.CurrentLevel;
            var levelDefinition = levels.FirstOrDefault(l => l.Reference == currentLevel.Reference);
            return levels.IndexOf(levelDefinition);
        }

        public bool IsNextLevelAvailable()
        {
            var levels = Base.Core.Game.State.Mode.Levels;
            int currentLevelIndex = GetCurrentLevelIndex();
            return currentLevelIndex < levels.Count - 1;
        }

        public bool IsPreviousLevelAvailable()
        {
            var levels = Base.Core.Game.State.Mode.Levels;
            int currentLevelIndex = GetCurrentLevelIndex();
            return currentLevelIndex > 0;
        }

        public void LoadNextLevel()
        {
            int currentLevelIndex = GetCurrentLevelIndex();
            LoadLevelByIndex(currentLevelIndex + 1);
        }

        public void LoadPreviousLevel()
        {
            int currentLevelIndex = GetCurrentLevelIndex();
            LoadLevelByIndex(currentLevelIndex - 1);
        }

        public void LoadLevelByIndex(int index)
        {
            var firstLevel = this.State.Mode.Levels[index];
            this.State.CurrentLevel = new LevelConverter().Convert(firstLevel);

            if (!this.State.LevelScores.TryGetValue(State.CurrentLevel.Reference, out var levelScore))
            {
                levelScore = new LevelScore();
                this.State.LevelScores[State.CurrentLevel.Reference] = levelScore;
            }

            this.State.CurrentLevel.Score = levelScore;
            Base.Core.Game.ChangeScene(Constants.Scenes.Game, false);
        }

    }
}
