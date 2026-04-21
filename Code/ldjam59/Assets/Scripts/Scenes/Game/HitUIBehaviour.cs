
using System;

using TMPro;

using UnityEngine;
namespace Assets.Scripts.Scenes.Game
{
    public class HitUIBehaviour : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text fastestHitTxt;
        [SerializeField]
        private TMP_Text leastHitTxt;
        [SerializeField]
        private TMP_Text totalLevelTxt;
        [SerializeField]
        private TMP_Text totalSentTxt;
        [SerializeField]
        private GameObject launchButtonOpen;
        [SerializeField]
        private GameObject launchButtonClose;

        private void OnEnable()
        {
            if (Base.Core.Game.IsLoaded)
            {
                var score = Base.Core.Game.State.CurrentLevel.Score;

                if (score.ShortestHitDuration == Double.MaxValue)
                {
                    launchButtonClose.SetActive(true);
                    launchButtonOpen.SetActive(false);
                    fastestHitTxt.text = "-";
                }
                else
                {

                    if (Base.Core.Game.IsNextLevelAvailable())
                    {
                        launchButtonClose.SetActive(false);
                        launchButtonOpen.SetActive(true);
                    }
                    fastestHitTxt.text = String.Format("{0:####0.0}s", score.ShortestHitDuration);
                }
                if (score.LeastSent == int.MaxValue)
                {
                    leastHitTxt.text = "-";
                }
                else
                {
                    leastHitTxt.text = score.LeastSent.ToString();
                }
                totalLevelTxt.text = String.Format("{0:####0.0}s", score.LevelTime);
                totalSentTxt.text = score.SignalsSend.ToString();
            }
        }
    }
}