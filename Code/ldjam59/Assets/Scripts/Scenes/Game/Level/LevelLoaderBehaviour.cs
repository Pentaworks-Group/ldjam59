using Assets.Scripts.Core.Models;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Scenes.Game.Level
{
    public class LevelLoaderBehaviour : MonoBehaviour
    {
        private int currentLevelIndex;
        private void Awake()
        {
            Base.Core.Game.ExecuteAfterInstantation(InitBehaviour);
        }

        private void InitBehaviour()
        {
            var levels = Base.Core.Game.State.Mode.Levels;
            var currentLevel = Base.Core.Game.State.CurrentLevel;
            var levelDefinition = levels.FirstOrDefault(l => l.Reference == currentLevel.Reference);
            currentLevelIndex = levels.IndexOf(levelDefinition);
        }

        public bool IsNextLevelAvailable()
        {
            var levels = Base.Core.Game.State.Mode.Levels;
            return currentLevelIndex < levels.Count - 1;
        }

        public bool IsPreviousLevelAvailable()
        {
            var levels = Base.Core.Game.State.Mode.Levels;
            return currentLevelIndex > 0;
        }

        public void LoadNextLevel()
        {
            LoadLevel(currentLevelIndex + 1);
        }

        public void LoadPreviousLevel()
        {
            LoadLevel(currentLevelIndex - 1);
        }

        private void LoadLevel(int index)
        {
            Base.Core.Game.PlayButtonSound();
            Base.Core.Game.LoadLevelByIndex(index);
        }
    }
}
