using System;
using System.Collections.Generic;

using TMPro;

using UnityEditor;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Prefabs.PauseMenu
{
    public class PauseMenuBehaviour : MonoBehaviour
    {
        private readonly Dictionary<GameObject, Boolean> oldActiveValues = new Dictionary<GameObject, Boolean>();

        private Boolean isPaused;
        private PauseSubMenuBehaviour openMenu;
        private InputAction escapeAction;

        public List<GameObject> hideOnPause = new List<GameObject>();
        public GameObject pauseMenuContainer;
        public PauseSubMenuBehaviour optionsMenu;
        public PauseSubMenuBehaviour saveGameMenu;
        public TMP_Text titleText;

        public void OnPauseToggled(Boolean isPaused)
        {
            this.isPaused = isPaused;

            if (!this.isPaused)
            {
                ShowItems();
                pauseMenuContainer.SetActive(false);
            }
            else
            {
                escapeAction.performed+= OnEscapePressed;

                HideItems();
                pauseMenuContainer.SetActive(true);
            }
        }

        private void OnEscapePressed(InputAction.CallbackContext context)
        {
            if (openMenu == default)
            {
                escapeAction.performed -= OnEscapePressed;
                Base.Core.Game.UnPause();
            }
            else
            {
                OpenMenu(default);
            }
        }

        public void OnOpenOptions() => OpenMenu(optionsMenu);
        public void OnOpenSaveGames() => OpenMenu(saveGameMenu);

        private void OpenMenu(PauseSubMenuBehaviour newValue)
        {
            if (openMenu != null)
            {
                openMenu.gameObject.SetActive(false);
            }

            openMenu = newValue;

            if (newValue != null)
            {
                titleText.text = newValue.title;

                newValue.gameObject.SetActive(true);
                pauseMenuContainer.SetActive(false);
            }
            else
            {
                pauseMenuContainer.SetActive(true);
                titleText.text = "Pause";
            }
        }

        private void HideItems()
        {
            oldActiveValues.Clear();

            foreach (var item in hideOnPause)
            {
                oldActiveValues[item] = item.activeSelf;
                item.SetActive(false);
            }
        }

        private void ShowItems()
        {
            foreach (var item in hideOnPause)
            {
                if (!oldActiveValues.TryGetValue(item, out var wasActive))
                {
                    wasActive = false;
                }

                item.SetActive(wasActive);
            }
        }

        private void Init()
        {
            Base.Core.Game.PauseToggled.AddListener(OnPauseToggled);
        }

        private void Awake()
        {
            escapeAction = InputSystem.actions.FindAction("Escape");

            Base.Core.Game.ExecuteAfterInstantation(Init);
        }
    }
}
