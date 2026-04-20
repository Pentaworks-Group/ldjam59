using System;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace Assets.Scripts.Scenes.Game.LevelPreview
{
    public class LevelListSlotBehaviour : GameFrame.Core.UI.List.ListSlotBehaviour<LevelPreview>
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
        private TMP_Text nameTxt;
        [SerializeField]
        private TMP_Text descriptionTxt;
        public override void RudeAwake()
        {

        }

        public override void UpdateUI()
        {
            var score = this.content.Score;
            if (score.ShortestHitDuration == Double.MaxValue)
            {
                fastestHitTxt.text = "-";
            }
            else
            {
                fastestHitTxt.text = String.Format("{0:####0.0}s", score.ShortestHitDuration);
            }
            totalLevelTxt.text = String.Format("{0:####0.0}s", score.LevelTime);
            totalSentTxt.text = score.SignalsSend.ToString();
            if (score.LeastSent == int.MaxValue)
            {
                leastHitTxt.text = "-";
            }
            else
            {
                leastHitTxt.text = score.LeastSent.ToString();
            }
            nameTxt.text = this.content.Name;
            descriptionTxt.text = this.content.Description;
        }
        public void LoadThisLevel()
        {
            Base.Core.Game.LoadLevelByIndex(this.content.Index);
        }
    }
}
