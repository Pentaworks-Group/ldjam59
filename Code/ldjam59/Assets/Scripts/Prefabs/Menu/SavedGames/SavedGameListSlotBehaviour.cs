using Assets.Scripts.Core.Persistence;

using GameFrame.Core.Extensions;

using UnityEngine.UI;

namespace Assets.Scripts.Prefabs.Menu.SavedGames
{
    public class SavedGameListSlotBehaviour : GameFrame.Core.UI.List.ListSlotBehaviour<SavedGamePreview>
    {
        private Image indicatorImage;

        public override void RudeAwake()
        {
            if (!transform.TryFindAndGetComponent("IndicatorContainer/IndicatorImage", out indicatorImage))
            {
                throw new System.Exception("Failed to get Text 'IndicatorContainer/IndicatorImage'!");
            }
        }

        public override void UpdateUI()
        {
            if (this.content.IsEmpty)
            {
                this.indicatorImage.color = UnityEngine.Color.red;
            }
            else
            {
                this.indicatorImage.color = UnityEngine.Color.green;
            }
        }
    }
}
