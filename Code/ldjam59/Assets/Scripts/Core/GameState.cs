using System;
using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.Core.Definitions;
using Assets.Scripts.Core.Models;


namespace Assets.Scripts.Core
{
    public class GameState : GameFrame.Core.GameState
    {
        private GameMode mode;
        public GameMode Mode
        {
            get
            {
                return mode;
            }
            set
            {
                if (mode != value)
                {
                    mode = value;
                }
            }
        }

        public Level CurrentLevel { get; set; }
        public Dictionary<string, LevelScore> LevelScores { get; set; } = new Dictionary<string, LevelScore>();
        public Double TimeElapsed { get; set; } = 0.0;
        public String DeathReason { get; set; }
        public Int32 Score { get; set; }
        public Int32 RemainingLives { get; set; }
        public Int32 MovementCounter { get; set; }
    }
}
