using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Scenes.MainMenu
{
    public class MainMenuBehaviour : GameFrame.Core.UI.Scripts.Menu.BaseMenuBehaviour
    {
        [SerializeField] private GameObject OptionsPopup;
        [SerializeField] private GameObject SavedGamesPopup;
        [SerializeField] private GameObject GameModesPopup;
        [SerializeField] private GameObject CreditsPopup;

        private GameObject currentPopup;

        private InputAction escapeAction;

        private void Awake()
        {
            escapeAction = InputSystem.actions.FindAction("Escape");
            escapeAction.performed += OnEscapePressed;
        }

        private void OnDisable()
        {
            escapeAction.performed -= OnEscapePressed;
        }

        private void OnEscapePressed(InputAction.CallbackContext context)
        {
            CloserPopus();
        }

        public void OnOpenOptions() => ShowPopup(OptionsPopup);
        public void OnOpenSavedGames() => ShowPopup(SavedGamesPopup);
        public void OnOpenCredits() => ShowPopup(CreditsPopup);

        public void OnOpenGameMode()
        {
            Base.Core.Game.PlayButtonSound();

            //GameModesPopup.SetActive(true);
            Base.Core.Game.Start();
        }

        public void OnQuit()
        {
            Base.Core.Game.Quit();
        }

        public void OnStartPopupClose()
        {
            string[] combinations = { "MenuMusic_1", "MenuMusic_2", "MenuMusic_3", "MenuMusic_3" };

            Base.Audio.AudioEngine.SetCombination("MenuMusic_2");
            Base.Audio.AudioEngine.Play();
            Base.Audio.AudioEngine.StartRandomCycling(combinations, 1, 7, 0.2f);
        }

        public void CloserPopus()
        {
            if (currentPopup != null)
            {
                currentPopup.SetActive(false);
            }            
        }

        private void ShowPopup(GameObject gameObject)
        {
            Base.Core.Game.PlayButtonSound();

            currentPopup = gameObject;
            currentPopup.SetActive(true);
        }
    }
}
