using Assets.Scripts.Core.Models;
using System;

namespace Assets.Scripts.Scenes.Game.Level
{
	public class LevelPreview
	{
        public String Reference { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Index { get; set; }
        public LevelScore Score { get; set; }
    }
}
