using Assets.Scripts.Core.Models;
using Newtonsoft.Json;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputSettings;


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
			Base.Core.Game.State.CurrentLevel = tt;
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
			Base.Core.Game.LoadLevelByIndex(index);
        }


	}
}