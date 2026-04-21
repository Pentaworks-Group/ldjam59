using UnityEngine;

namespace Assets.Scripts.Scenes.Game.Level
{
	public class LevelHandlerBehaviour : MonoBehaviour
	{
		public void LoadNextLevel()
		{
			Base.Core.Game.LoadNextLevel();
		}

		public void LoadPreviousLevel()
		{
			Base.Core.Game.LoadPreviousLevel();
		}
	}
}
