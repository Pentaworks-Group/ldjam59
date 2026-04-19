using UnityEngine;
namespace Assets.Scripts.Scenes.Game
{
    public class StaticUIBehaviour : MonoBehaviour
    {
        [SerializeField]
        private GameMainBehaviour main;
        [SerializeField]
        private GameObject container;

        private void Awake()
        {
            main.OnTargetHit.AddListener(TargetHit);
        }

        private void TargetHit()
        {
            container.SetActive(true);
        }

        public void CloseHitWindow()
        {
            container.SetActive(false);
        }
    }
}