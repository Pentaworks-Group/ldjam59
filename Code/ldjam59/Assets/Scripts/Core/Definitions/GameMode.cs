using System.Collections.Generic;

namespace Assets.Scripts.Core.Definitions
{
    public class GameMode : GameFrame.Core.Definitions.GameMode
    {
        public AudioDefinition Audio { get; set; }
        public List<LevelDefinition> Levels { get; set; }
    }
}
