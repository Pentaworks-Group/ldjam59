using UnityEngine;

namespace Assets.Scripts.Scenes.Game.Admin
{
	public class AdminBehaviour : MonoBehaviour
	{
		[SerializeField]
		private GameObject panel;
		[SerializeField]
		private GameObject button;

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