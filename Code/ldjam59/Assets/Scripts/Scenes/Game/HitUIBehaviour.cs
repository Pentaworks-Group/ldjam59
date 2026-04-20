using System;

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

            var currentLevel = Base.Core.Game.State.CurrentLevel;
            var score = currentLevel.Score;

            if (score.LeastSent > currentLevel.SignalsSend)
            {
                score.LeastSent = currentLevel.SignalsSend;
            }

            if (score.ShortestHitDuration > currentLevel.TimeElapsed)
            {
                score.ShortestHitDuration = currentLevel.TimeElapsed;
            }
            
            container.SetActive(true);
            pauseButton.SetActive(false);

            fastestHitTxt.text = String.Format("{0:####0.0}s", score.ShortestHitDuration);
            leastHitTxt.text = score.LeastSent.ToString();
            totalLevelTxt.text = String.Format("{0:####0.0}s", score.LevelTime);
            totalSentTxt.text = score.SignalsSend.ToString();
        }

        public void CloseHitWindow()
        {
            container.SetActive(false);
            pauseButton.SetActive(true);
            Time.timeScale = (previousTimeScale.HasValue ? previousTimeScale.Value : 1f);
        }
    }
}