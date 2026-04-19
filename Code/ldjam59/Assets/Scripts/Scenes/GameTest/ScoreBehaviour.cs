using TMPro;

using UnityEngine;

public class ScoreBehaviour : MonoBehaviour
{
    public TMP_Text text;
    private float elapsed;

    private int score;

    private void Update()
    {
        text.text = $"Score: {score}";
    }

    public void AddScore()
    {
        score++;
    }
}