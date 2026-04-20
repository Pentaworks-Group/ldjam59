using System.Linq;

using Assets.Scripts.Core.Models;

using Newtonsoft.Json;

using TMPro;

using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts.Scenes.Game.Admin
{
    public class AdminBehaviour : MonoBehaviour
    {
        [SerializeField]
        private GameObject panel;
        [SerializeField]
        private GameObject button;
        [SerializeField]
        private TMP_InputField jsonField;
        [SerializeField]
        private Button nextLevelButton;
        [SerializeField]
        private Button previousLevelButton;

        private int currentLevelIndex;

        private void Awake()
        {
            Base.Core.Game.ExecuteAfterInstantation(InitBehaviour);
        }

        private void InitBehaviour()
        {
            EnableDisableLevelButtons();
        }

        public void GenerateJson()
        {

            Base.Core.Game.OnModelUpdate.Invoke();

            var cleanedLevel = Base.Core.Game.State.CurrentLevel;
            var json = GameFrame.Core.Json.Handler.Serialize(cleanedLevel, Formatting.Indented, new JsonSerializerSettings());

            jsonField.text = json;
        }

        public void LoadJson()
        {
            var json = jsonField.text;

            var tt = GameFrame.Core.Json.Handler.Deserialize<Level>(json);
            Core.GameState state = Base.Core.Game.State;
            state.CurrentLevel = tt;
            if (!state.LevelScores.TryGetValue(tt.Reference, out var levelScore))
            {
                levelScore = new LevelScore();
                state.LevelScores[tt.Reference] = levelScore;
            }
            tt.Score = levelScore;
            Base.Core.Game.ChangeScene(Constants.Scenes.Game, false);
        }

        public void OpenPanel()
        {
            panel.SetActive(true);
            button.SetActive(false);
        }

        public void ClosePanel()
        {
            panel.SetActive(false);
            button.SetActive(true);
        }

        private void EnableDisableLevelButtons()
        {
            var levels = Base.Core.Game.State.Mode.Levels;
            var currentLevel = Base.Core.Game.State.CurrentLevel;
            var levelDefinition = levels.FirstOrDefault(l => l.Reference == currentLevel.Reference);
            currentLevelIndex = levels.IndexOf(levelDefinition);

            if (currentLevelIndex > 0)
            {
                previousLevelButton.interactable = true;
            }
            else
            {
                previousLevelButton.interactable = false;
            }

            if (currentLevelIndex < levels.Count - 1)
            {
                nextLevelButton.interactable = true;
            }
            else
            {
                nextLevelButton.interactable = false;
            }
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