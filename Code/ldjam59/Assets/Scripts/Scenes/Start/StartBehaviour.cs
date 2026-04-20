using UnityEngine;

namespace Assets.Scripts.Scenes.Start
{
    public class StartBehaviour : MonoBehaviour
    {
        private void Awake()
        {
            _ = Base.Audio.AudioEngine;
        }

        public void OnClick()
        {
            Base.Core.Game.ChangeScene(Constants.Scenes.MainMenu);
        }
    }
}
