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
            //            audioEngine.SetCombination("MenuMusic");
            string[] combinations = {"MenuMusic", "Level_0"};
            audioEngine.StartRandomCycling(combinations, 1, 7, 0.2f);
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
