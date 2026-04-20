using TMPro;
using UnityEngine;
namespace Assets.Scripts.Scenes.Game
{
    public class HitUIBehaviour : MonoBehaviour
    {
        [SerializeField]
        private GameMainBehaviour main;
        [SerializeField]
        private GameObject container;
        [SerializeField]
        private GameObject pauseButton;
        [SerializeField]
        private TMP_Text fastestHitTxt;
        [SerializeField]
        private TMP_Text leastHitTxt;
        [SerializeField]
        private TMP_Text totalLevelTxt;
        [SerializeField]
        private TMP_Text totalSentTxt;

        protected float? previousTimeScale;

        private void Awake()
        {
            main.OnTargetHit.AddListener(TargetHit);
        }

        private void TargetHit()
        {
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            container.SetActive(true);
            pauseButton.SetActive(false);
            var levelScore = Base.Core.Game.State.CurrentLevel.Score;
            fastestHitTxt.text = levelScore.ShortestHitDuration.ToString();
            leastHitTxt.text = levelScore.LeastSent.ToString();
            totalLevelTxt.text = levelScore.LevelTime.ToString();
            totalSentTxt.text = levelScore.SignalsSend.ToString();
        }

        public void CloseHitWindow()
        {
            container.SetActive(false);
            pauseButton.SetActive(true);
            Time.timeScale = (previousTimeScale.HasValue ? previousTimeScale.Value : 1f);
        }
    }
}