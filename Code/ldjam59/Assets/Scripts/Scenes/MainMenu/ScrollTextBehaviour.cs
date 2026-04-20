using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

namespace Assets.Scripts.Scenes.MainMenu
{
    [RequireComponent(typeof(TMP_Text))]
    public class ScrolltextBehaviour : MonoBehaviour
    {
        [SerializeField] private TMP_Text textField;
        [SerializeField] private int height = 10;
        //[SerializeField] private int width = 16;
        [SerializeField] private float updateInterval = 1;

        private string[] lines;
        private int nextIndex = 0;
        private readonly List<string> currentLines = new List<string>();

        private Boolean isRunning;

        private void Start()
        {
            Debug.Log($"Time: {Time.timeScale}");

            lines = SplitToLines(textField.text);
            int end = Mathf.Min(nextIndex + height, lines.Length);

            for (int i = nextIndex; i < end; i++)
            {
                currentLines.Add(lines[i]);
            }

            textField.text = string.Join("\n", currentLines);

            nextIndex = end;

            if (end == height)
            {
                this.isRunning = true;
                StartCoroutine(Timer());
            }
        }

        private void OnEnable()
        {
            if (this.isRunning)
            {
                StartCoroutine(Timer());
            }
        }

        private IEnumerator Timer()
        {
            WaitForSeconds wait = new WaitForSeconds(updateInterval);

            while (true)
            {
                UpdateText();
                yield return wait;
            }
        }

        private static string[] SplitToLines(string rawText)
        {
            string[] lines = rawText.Split(new string[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.None);

            return lines;
        }

        private void UpdateText()
        {
            currentLines.RemoveAt(0);
            currentLines.Add(lines[nextIndex]);

            textField.text = string.Join("\n", currentLines);

            nextIndex++;

            if (nextIndex >= lines.Length)
            {
                nextIndex = 0;
            }
        }
    }
}
