using System;

namespace Assets.Scripts.Core.Models
{
    public class LevelScore
    {
        public Int32 SignalsSend { get; set; }
        public Double LevelTime { get; set; }
        public Double ShortestHitDuration { get; set; } = Double.MaxValue;
        public Int32 LeastSent { get; set; } = Int32.MaxValue;
    }
}
