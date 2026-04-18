using System.Collections.Generic;

namespace Assets.Scripts.Core.Definitions
{
    public class GameMode : GameFrame.Core.Definitions.GameMode
    {
        public List<LevelDefinition> Levels { get; set; }
    }
}
