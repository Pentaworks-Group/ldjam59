using System;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

namespace Assets.Scripts.Scenes.MainMenu
{
    [RequireComponent(typeof(TMP_Text))]
    public class ScrollingTextBehaviour : MonoBehaviour
    {
        [SerializeField] private TMP_Text textField;
        [SerializeField] private float startDelay = 1f;
        [SerializeField] private float updateInterval = 1;

        
        private readonly List<String> currentLines = new List<String>();

        private Boolean checkedScroll = false;
        private Boolean? isFittingOnScreen = false;
        private String sourceText;

        private void InitializeText(String text)
        {
            this.sourceText = text;

            currentLines.Clear();

            if (!String.IsNullOrEmpty(text))
            {
                var lines = SplitToLines(sourceText);

                currentLines.AddRange(lines);
            }
        }

        private void Start()
        {
            InitializeText(textField.text);
        }

        private void UpdateText()
        {
            if (currentLines.Count > 0)
            {
                var entry = currentLines[0];
                
                _ = currentLines.Remove(entry);
                currentLines.Add(entry);

                textField.text = String.Join("\n", currentLines);
            }
        }

        private void Update()
        {
            if (textField.text.Length != sourceText.Length)
            {
                if (checkedScroll && isFittingOnScreen == false)
                {
                    CancelInvoke(nameof(UpdateText));
                }

                InitializeText(textField.text);
                checkedScroll = false;

                isFittingOnScreen = default;
            }

            if (!checkedScroll)
            {
                checkedScroll = true;

                if (currentLines.Count > textField.textInfo.lineCount)
                {
                    InvokeRepeating(nameof(UpdateText), startDelay, updateInterval);
                }
                else
                {
                    isFittingOnScreen = true;
                }
            }
        }

        private static String[] SplitToLines(String rawText)
        {
            return rawText.Split(new String[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.None);
        }
    }
}
