using System;

namespace Assets.Scripts.Core.Persistence
{
    public class SavedGamePreview : GameFrame.Core.Persistence.SavedGamePreview<GameState>
    {
        public Boolean IsEmpty{ get; set; }
        public DateTime SavedOn { get; set; }
        public Double TimeElapsed { get; set; }
        public String Level { get; set; }

        public override void Init(GameState gameState, String key)
        {
            base.Init(gameState, key);

            this.IsEmpty = false;
            this.SavedOn = gameState.SavedOn;
            this.TimeElapsed = gameState.TimeElapsed;
            this.Level = gameState.CurrentLevel.Name;
        }
    }
}
