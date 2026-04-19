using System;

using GameFrame.Core;

using UnityEngine;

namespace Assets.Scripts.Scenes.MainMenu
{
    public class MainMenuBehaviour : GameFrame.Core.UI.Scripts.Menu.BaseMenuBehaviour
    {
        [SerializeField] AudioEngine audioEngine;

        private void Awake()
        {
            audioEngine.Play();
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
