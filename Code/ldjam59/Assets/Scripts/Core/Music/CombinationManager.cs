using System.Collections;
using UnityEngine;

public class CombinationManager : MonoBehaviour
{
    [Header("References")]
    public MultitrackPlayer player;
    public MultitrackPlayerState state;

    [Header("Combinations")]
    public TrackCombination[] combinations;

    private Coroutine[] fadeCoroutines;
    private int activeCobinationIndex = -1;

    void Awake()
    {
        fadeCoroutines = new Coroutine[state.tracks.Length];
    }

    public void ActivateCombination(int index)
    {
        if (!isValidIndex(index)) return;
        activeCobinationIndex = index;

        var combo = combinations[index];
        for (int i = 0; i < state.tracks.Length; i++)
        {
            var entry = getEntry(combo, i);
            applyEntry(i, entry.active ? entry.volume : 0f, entry.pan);
        }
    }

    public void ActivateCombination(string name)
    {
        int index = findCombinationIndex(name);
        if (index >= 0) ActivateCombination(index);
        else Debug.LogWarning($"Combination '{name}' not found.");
    }

    public void ActivateCombinationFaded(int index, float duration)
    {
        if (!isValidIndex(index)) return;
        activeCobinationIndex = index;

        var combo = combinations[index];
        for (int i = 0;i < state.tracks.Length;i++)
        {
            var entry = getEntry(combo, i);
            float targetVolume = entry.active ? entry.volume :0f;
            startFade(i, targetVolume, entry.pan, duration);
        }
    }

    public void ActivateCombinationFaded(string name, float duration)
    {
        int index = findCombinationIndex(name);
        if (index >= 0) ActivateCombinationFaded(index, duration);
        else Debug.LogWarning($"Combination '{name}' not found.");
    }

    public void FadeOutAll(float duration)
    {
        for (int i = 0; i < state.tracks.Length;i++)
        {
            var src = player.GetSource(i);
            float currentPan = src != null ? src.panStereo : 0f;
            startFade(i, 0f, currentPan, duration);
        }
    }

    public void FadeInCurrent(float duration)
    {
        if (activeCobinationIndex >= 0)
            ActivateCombinationFaded(activeCobinationIndex, duration);
    }

    public int GetActiveCombinationIndex() => activeCobinationIndex;

    private void applyEntry(int index, float volume, float pan)
    {
        var src = player.GetSource(index);
        if (src == null) return;

        src.volume = volume;
        src.panStereo = pan;

        // Keep AudioTrackData in sync
        state.tracks[index].volume = volume;
        state.tracks[index].pan = pan;
        state.tracks[index].muted = (volume <= 0f);
    }

    private void startFade(int trackIndex, float targetVolume, float targetPan, float duration)
    {
        if (fadeCoroutines[trackIndex] != null)
            StopCoroutine(fadeCoroutines[trackIndex]);

        fadeCoroutines[trackIndex] = StartCoroutine(
            fadeTrack(trackIndex, targetVolume, targetPan, duration)
        );
    }

    private IEnumerator fadeTrack(int trackIndex, float targetVolume, float targetPan, float duration)
    {
        var src = player.GetSource(trackIndex);
        if (src == null) yield break;

        float startVolume = src.volume;
        float startPan = src.panStereo;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);

            float newVolume = Mathf.Lerp(startVolume, targetVolume, t);
            float newPan = Mathf.Lerp(startPan, targetPan, t);

            // Keep state in sync every frame during fade
            applyEntry(trackIndex, newVolume, newPan);

            yield return null;
        }

        // Snap to exact target at end
        applyEntry(trackIndex, targetVolume, targetPan);

    }

    private TrackCombinationEntry getEntry(TrackCombination combo, int trackIndex)
    {
        if (trackIndex < combo.tracks.Length)
            return combo.tracks[trackIndex];

        return new TrackCombinationEntry { active = false, volume = 0f, pan = 0f };
    }

    private int findCombinationIndex(string name)
    {
        for (int i = 0; i < combinations.Length; i++)
            if (combinations[i].combinationName == name) return i;
        return -1;
    }

    private bool isValidIndex(int index) =>
        index >= 0 && index < combinations.Length;
}
