using Assets.Scripts.Core.Models;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

using Assets.Scripts.Constants;

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


        public void GenerateJson()
		{
			var cleanedLevel = Base.Core.Game.State.CurrentLevel;
            var json = GameFrame.Core.Json.Handler.Serialize(cleanedLevel, Formatting.Indented, new JsonSerializerSettings());

            jsonField.text = json;
        }


        public void LoadJson()
        {
			var json = jsonField.text;

			var tt = GameFrame.Core.Json.Handler.Deserialize<Level>(json);
			Base.Core.Game.State.CurrentLevel = tt;
			Base.Core.Game.ChangeScene("Game", true);
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