using System;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

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
        private String currentText;
        private Boolean isInvoking = false;

        private void InitializeText(String text)
        {
            currentLines.Clear();

            if (!String.IsNullOrEmpty(text))
            {
                var lines = SplitToLines(text);

                currentLines.AddRange(lines);

                currentText = text;
            }
            else
            {
                this.currentText = "";
            }
        }

        private void Start()
        {
            InitializeText(textField.text);
        }

        private void Update()
        {
            if (textField.text != currentText)
            {
                if (checkedScroll)
                {
                    //CancelInvoke(nameof(UpdateText));
                    CancelInvoke();
                    isInvoking = false;
                }

                InitializeText(textField.text);
                checkedScroll = false;
            }
        }

        private void LateUpdate()
        {
            if (!checkedScroll)
            {
                checkedScroll = true;

                if (!string.IsNullOrEmpty(textField.text) && textField.textInfo.lineCount == 0)
                {
                    textField.ForceMeshUpdate();
                }

                if (textField.isTextOverflowing)
                {
                    if (!isInvoking)
                    {
                        InvokeRepeating(nameof(UpdateText), startDelay, updateInterval);
                        isInvoking = true;
                    }
                }
                else
                {
                    CancelInvoke();
                    isInvoking = false;
                }
            }
        }

        private void UpdateText()
        {
            if (currentLines.Count > 0)
            {
                var entry = currentLines[0];

                _ = currentLines.Remove(entry);
                currentLines.Add(entry);

                this.currentText = String.Join("\n", currentLines);

                textField.text = this.currentText;
            }
            else
            {
                this.currentText = "";
            }
        }

        private static String[] SplitToLines(String rawText)
        {
            return rawText.Split(new String[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.None);
        }
    }
}
