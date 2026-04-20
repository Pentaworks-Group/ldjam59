
using NUnit;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Scenes.MainMenu
{
    public class CreditsBehaviour : MonoBehaviour
    {
        [SerializeField] private TMP_Text textField;
        [SerializeField] private int height = 10;
        //[SerializeField] private int width = 16;
        [SerializeField] private float updateInterval = 1;
        private string[] lines;
        private int nextIndex = 0;
        private List<string> currentLines = new List<string>();

        private void Start()
        {
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
