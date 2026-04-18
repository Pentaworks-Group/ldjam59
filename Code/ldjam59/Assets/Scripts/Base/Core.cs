using System;

using UnityEngine;
using Assets.Scripts.Core;

namespace Assets.Scripts.Base
{
    public class Core
    {
        private static readonly Lazy<Scripts.Core.Game> lazyGame = new Lazy<Scripts.Core.Game>(() => GameFrame.Core.Core.GetStartedGame<Scripts.Core.Game, GameState, PlayerOptions>(), true);
        public static Assets.Scripts.Core.Game Game
        {
            get
            {
                return lazyGame.Value;
            }
        }
    }
}