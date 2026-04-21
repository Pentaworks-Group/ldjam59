using TMPro;

using UnityEngine;

namespace Assets.Scripts.Scenes.Start
{
    public class StartBehaviour : MonoBehaviour
    {
        [SerializeField] private TMP_Text versionText;

        private void Awake()
        {
            _ = Base.Audio.AudioEngine;

            versionText.text = $"{Application.version}";
        }

        public void OnClick()
        {
            Base.Core.Game.ChangeScene(Constants.Scenes.MainMenu);
        }
    }
}
