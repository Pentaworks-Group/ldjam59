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

                    contentBuilder.AppendLine("The selected drive contains some silly data.");
                    contentBuilder.AppendLine("Suggested information:");
                    contentBuilder.AppendLine("-------------");
                    contentBuilder.AppendLine("Created on:");
                    contentBuilder.AppendLine(String.Format("{0:d} at {0:t}.", currentSlot.SavedOn));
                    contentBuilder.AppendLine("");
                    contentBuilder.AppendLine("Level:");
                    contentBuilder.AppendLine(String.Format("{0}", currentSlot.Level));
                    contentBuilder.AppendLine("");
                    contentBuilder.AppendLine("Time wasted:");
                    contentBuilder.AppendLine(String.Format("{0:###0.0}s", currentSlot.TimeElapsed));
                    contentBuilder.AppendLine("-------------");
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
