using Assets.Scripts.Core.Models;
using Assets.Scripts.Scenes.Game.Level;
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


        private void OnEnable()
        {
            EnableDisableLevelButtons();
        }

        private void EnableDisableLevelButtons()
        {

            if (Base.Core.Game.IsPreviousLevelAvailable())
            {
                previousLevelButton.interactable = true;
            }
            else
            {
                previousLevelButton.interactable = false;
            }

            if (Base.Core.Game.IsNextLevelAvailable())
            {
                nextLevelButton.interactable = true;
            }
            else
            {
                nextLevelButton.interactable = false;
            }
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

            var tt = GameFrame.Core.Json.Handler.Deserialize<Assets.Scripts.Core.Models.Level>(json);
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


    }
}