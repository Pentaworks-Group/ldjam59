using System;
using System.Collections.Generic;

using TMPro;

using UnityEditor;
using UnityEditor.Analytics;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Prefabs.Menu.Pause
{
    public class PauseMenuBehaviour : MonoBehaviour
    {
        private readonly Dictionary<GameObject, Boolean> oldActiveValues = new Dictionary<GameObject, Boolean>();


        private PauseSubMenuBehaviour openMenu;
        private InputAction escapeAction;

        public List<GameObject> hideOnPause = new List<GameObject>();
        public GameObject pauseMenuContainer;
        public PauseSubMenuBehaviour defaultMenu;
        public TMP_Text titleText;

        private bool isOpen = false;

        private void OnDisable()
        {
            escapeAction.performed -= OnEscapePressedInMenu;
            escapeAction.performed -= OnEscapePressedOutsideMenu;
        }

        private void OnEscapePressedInMenu(InputAction.CallbackContext context)
        {
            if (openMenu == default)
            {
                ClosePauseMenu();
            }
            else
            {
                OnBack();
            }
        }

        private void OnEscapePressedOutsideMenu(InputAction.CallbackContext context)
        {
            OpenPauseMenu();
        }

        public void ClosePauseMenu()
        {
            isOpen = false;
            escapeAction.performed -= OnEscapePressedInMenu;
            escapeAction.performed += OnEscapePressedOutsideMenu;
            ShowItems();
            pauseMenuContainer.SetActive(false);
            Base.Core.Game.UnPause();
        }

        public void OpenPauseMenu()
        {
            isOpen = true;
            escapeAction.performed += OnEscapePressedInMenu;
            escapeAction.performed -= OnEscapePressedOutsideMenu;

            HideItems();

            titleText.text = defaultMenu.title;
            defaultMenu.gameObject.SetActive(true);
            pauseMenuContainer.SetActive(true);
            Base.Core.Game.Pause();
        }

        public void OnBack()
        {
            if (openMenu == default)
            {
                ClosePauseMenu();
                return;
            }
            OpenMenu(default);
        }


        public void OnQuit()
        {
            Base.Core.Game.PlayButtonSound();

            Base.Core.Game.Stop();
            Base.Core.Game.ChangeScene(Constants.Scenes.MainMenu);

            Time.timeScale = 1;
        }

        public void OpenMenu(PauseSubMenuBehaviour newValue)
        {
            if (!isOpen)
            {
                OpenPauseMenu();
            }
            if (openMenu != null)
            {
                openMenu.gameObject.SetActive(false);
            }

            openMenu = newValue;

            if (newValue != null)
            {
                titleText.text = newValue.title;

                newValue.gameObject.SetActive(true);
                defaultMenu.gameObject.SetActive(false);
            }
            else
            {
                defaultMenu.gameObject.SetActive(true);
                titleText.text = defaultMenu.title;
            }

            Base.Core.Game.PlayButtonSound();
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

        //private void Init()
        //{
        //    Base.Core.Game.PauseToggled.AddListener(OnPauseToggled);
        //}

        private void Awake()
        {
            escapeAction = InputSystem.actions.FindAction("Escape");
            escapeAction.performed += OnEscapePressedOutsideMenu;

            //Base.Core.Game.ExecuteAfterInstantation(Init);
        }
    }
}
