using System;
using System.Text;

using Assets.Scripts.Core.Persistence;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Prefabs.Menu.SavedGames
{
    public class SavedGamesBehaviour : MonoBehaviour
    {
        [SerializeField] private LevelListContainerBehaviour listContainer;
        [SerializeField] private Button eraseButton;
        [SerializeField] private Button saveButton;
        [SerializeField] private Button loadButton;
        [SerializeField] private TMP_Text displayText;

        private SavedGamePreview currentSlot;
        private Boolean isSaveAllowed = true;

        public void SaveGame()
        {
            //Base.Core.Game.SaveGame();

            if (this.currentSlot != default)
            {
                Base.Core.Game.PlayButtonSound();
                Base.Core.Game.OverwriteSavedGame(this.currentSlot.Key);
            }

            listContainer.UpdateList();
        }

        public void DeleteSelectedSavedGame()
        {
            if (currentSlot != null)
            {
                Base.Core.Game.DeleteSavedGame(currentSlot.Key);
                listContainer.UpdateList();

                Base.Core.Game.PlayButtonSound();

                UpdateDetails(default);
                this.currentSlot = null;
            }
        }

        public void LoadSelectedSavedGame()
        {
            if (currentSlot != null)
            {
                Base.Core.Game.Stop();
                Base.Core.Game.PlayButtonSound();
                Base.Core.Game.LoadSavedGame(currentSlot.Key);
                Base.Core.Game.ForceSceneChange(Constants.Scenes.Game);
            }
        }

        public void OnSlotSelected(SavedGamePreview selectedSlot)
        {
            Base.Core.Game.PlayButtonSound();

            currentSlot = selectedSlot;

            UpdateDetails(selectedSlot);
        }

        private void UpdateDetails(SavedGamePreview selectedSlot)
        {
            if (selectedSlot != default)
            {
                if (isSaveAllowed)
                {
                    saveButton.interactable = true;
                }

                var contentBuilder = new StringBuilder();

                contentBuilder.AppendLine($"Slot: {currentSlot.Key}");

                if (selectedSlot.IsEmpty)
                {
                    contentBuilder.AppendLine("Empty");
                    
                    eraseButton.interactable = false;
                    loadButton.interactable = false;
                }
                else
                {
                    eraseButton.interactable = true;
                    loadButton.interactable = true;

                    contentBuilder.AppendLine(String.Format("The selected drive contains some silly data.\r\nIt appears to have been stored on {0:d} at {0:t}.\r\nThe data suggests information such as \"Level\" ({1}) and \"Time wasted\" ({2:###0.0}s).", currentSlot.SavedOn, currentSlot.Level, currentSlot.TimeElapsed));
                }

                displayText.text = contentBuilder.ToString();
            }
            else
            {
                saveButton.interactable = false;
                loadButton.interactable = false;
                eraseButton.interactable = false;

                displayText.text = "";
            }
        }

        private void Awake()
        {
            if (saveButton != null)
            {
                if (Base.Core.Game.State != default)
                {
                    saveButton.interactable = true;
                    isSaveAllowed = true;
                }
                else
                {
                    saveButton.interactable = false;
                    isSaveAllowed = false;
                }
            }

            displayText.text = "";
        }
    }
}
