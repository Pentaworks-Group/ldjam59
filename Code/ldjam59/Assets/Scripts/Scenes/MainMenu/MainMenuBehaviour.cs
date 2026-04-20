using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Scenes.MainMenu
{
    public class MainMenuBehaviour : GameFrame.Core.UI.Scripts.Menu.BaseMenuBehaviour
    {
        [SerializeField] AudioEngine audioEngine;
        [SerializeField] private GameObject OptionsPopup;
        [SerializeField] private GameObject SavedGamesPopup;
        [SerializeField] private GameObject GameModesPopup;

        private InputAction escapeAction;

        private void Awake()
        {
            string[] combinations = { "MenuMusic_1", "MenuMusic_2", "MenuMusic_3", "MenuMusic_3" };
            audioEngine.StartRandomCycling(combinations, 1, 7, 0.2f);
            audioEngine.Play();
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
        public void OnOpenOptions()
        {
            OptionsPopup.SetActive(true);
        }
        public void OnOpenSavedGames()
        {
            SavedGamesPopup.SetActive(true);
        }
        public void OnOpenGameMode()
        {
            //GameModesPopup.SetActive(true);
            Base.Core.Game.Start();
        }
        public void OnQuit()
        {
            Base.Core.Game.Quit();
        }

        public void CloserPopus()
        {
            OptionsPopup.SetActive(false);
            SavedGamesPopup.SetActive(false);
            GameModesPopup.SetActive(false);
            Debug.Log("Closing popups");
        }
    }
}
