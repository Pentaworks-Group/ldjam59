using UnityEngine;

public class CombinationTestUI : MonoBehaviour
{
    public CombinationManager manager;
    public float fadeDuration = 2f;

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(330, 20, 320, 500));
        GUILayout.Label("-- Kombinationen --");

        GUILayout.BeginHorizontal();
        GUILayout.Label("Fade (s): ", GUILayout.Width(60));
        float.TryParse(
            GUILayout.TextField(fadeDuration.ToString("F1"), GUILayout.Width(50)),
            out fadeDuration
        );
        GUILayout.EndHorizontal();

        GUILayout.Space(6);

        for (int  i = 0; i < manager.combinations.Length; i++)
        {
            var combo = manager.combinations[i];
            bool isActive = manager.GetActiveCombinationIndex() == i;

            GUILayout.BeginHorizontal();
            GUI.color = isActive ? Color.green : Color.white;
            if (GUILayout.Button($"Fade {fadeDuration:F1}s", GUILayout.Width(90), GUILayout.Height(36)))
                manager.ActivateCombinationFaded(i, fadeDuration);
            GUILayout.EndHorizontal();
            
            if (isActive)
            {
                for (int j = 0; j < combo.tracks.Length; j++)
                {
                    var entry = combo.tracks[j];
                    string trackName = j < manager.state.tracks.Length
                        ? manager.state.tracks[j].trackName : $"Track {j}";

                    GUI.color = entry.active ? Color.white : Color.gray;
                    GUILayout.Label(
                        $"  {trackName}: Vol {entry.volume:F2} Pan {entry.pan:F2}",
                        GUILayout.Height(18)
                    );
                    GUI.color = Color.white;
                }
                GUILayout.Space(4);
            }
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Fade Out All", GUILayout.Height(36))) manager.FadeOutAll(fadeDuration);
        if (GUILayout.Button("Fade In Current", GUILayout.Height(36))) manager.FadeInCurrent(fadeDuration);

        GUILayout.EndArea();
    }
}
