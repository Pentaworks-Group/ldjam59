
using System;

using GameFrame.Core;

using UnityEngine;

namespace Assets.Scripts.Scenes.MainMenu
{
    public class MainMenuBehaviour : GameFrame.Core.UI.Scripts.Menu.BaseMenuBehaviour
    {
        [SerializeField] MultitrackPlayer multiTrackPlayer;
        [SerializeField] CombinationManager combinationManager;

        private void Awake()
        {
            multiTrackPlayer.Play();
        }

        public void OnOpenOptions() => ChangeToScene(Constants.Scenes.Options);
        public void OnOpenCredits() => ChangeToScene(Constants.Scenes.Credits);
        public void OnOpenSavedGames() => ChangeToScene(Constants.Scenes.SavedGames);
        public void OnOpenGameMode() => ChangeToScene(Constants.Scenes.GameMode);
        public void OnQuit()
        {
            Base.Core.Game.Quit();
        }
    }
}
