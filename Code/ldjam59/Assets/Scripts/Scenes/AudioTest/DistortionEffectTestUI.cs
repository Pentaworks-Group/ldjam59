using UnityEngine;

public class DistortionEffectTestUI : MonoBehaviour
{
    public DistortionEffect distortionEffect;
    public int[] selectedTracks = { 0 };

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(650, 340, 300, 250));
        GUILayout.Label("-- Distortion Effect --");

        distortionEffect.intensity = LabeledSlider("Intensity", distortionEffect.intensity, 0f, 1f);
        distortionEffect.intensity = LabeledSlider("Crackle", distortionEffect.crackleRate, 0f, 1f);
        distortionEffect.intensity = LabeledSlider("Fade (s)", distortionEffect.fadeDuration, 0f, 5f);

        GUILayout.Space(6);

        if (GUILayout.Button("Enable (all tracks)", GUILayout.Height(32))) distortionEffect.EnableRadioEffect();
        if (GUILayout.Button("Enable (selected)", GUILayout.Height(32))) distortionEffect.EnableRadioEffect(selectedTracks);
        if (GUILayout.Button("Disable", GUILayout.Height(32))) distortionEffect.DisableRadioEffect();
        if (GUILayout.Button("Signal Loss (3s)", GUILayout.Height(32))) distortionEffect.TriggerSignalLoss(3);

        GUILayout.EndArea();
    }

    float LabeledSlider(string label, float value, float minValue, float maxValue)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label, GUILayout.Width(80));
        value = GUILayout.HorizontalSlider(value, minValue, maxValue);
        GUILayout.Label(value.ToString("F2"), GUILayout.Width(36));
        GUILayout.EndHorizontal();
        return value;
    }    
}
