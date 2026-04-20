using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.UI.CanvasScaler;

public class CombinationManager : MonoBehaviour
{
    [Header("References")]
    public MultitrackPlayer player;
    public MultitrackPlayerState state;

    [Header("Combinations")]
    public TrackCombination[] combinations;

    private Coroutine[] fadeCoroutines;
    private int activeCobinationIndex = -1;

    private void Awake()
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
        for (int i = 0; i < state.tracks.Length; i++)
        {
            var entry = getEntry(combo, i);
            float targetVolume = entry.active ? entry.volume : 0f;
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
            for (int i = 0; i < state.tracks.Length; i++)
            startFade(i, 0f, state.tracks[i].pan, duration);
    }

    public void FadeInCurrent(float duration)
    {
        if (activeCobinationIndex >= 0)
            ActivateCombinationFaded(activeCobinationIndex, duration);
    }

    public int GetActiveCombinationIndex() => activeCobinationIndex;

    // Writes logical volume/pan into state and lets MultitrackPlayer own src.volume.
    private void applyEntry(int index, float volume, float pan)
    {
        var track = state.tracks[index];
        track.volume = volume;
        track.pan = pan;
        track.muted = (volume <= 0f);

        // Single authority for writing to the AudioSource
        player.ApplyTrackSettings(index);
    }

    private void startFade(int trackIndex, float targetVolume, float targetPan, float duration)
    {
        if (fadeCoroutines != null)
        {
            if (fadeCoroutines[trackIndex] != null)
            {
                StopCoroutine(fadeCoroutines[trackIndex]);
            }

            fadeCoroutines[trackIndex] = StartCoroutine(fadeTrack(trackIndex, targetVolume, targetPan, duration));
        }
    }

    private IEnumerator fadeTrack(int trackIndex, float targetVolume, float targetPan, float duration)
    {
        // Read logical volume from state — this is always correct regardless of masterVolume.
        float startVolume = state.tracks[trackIndex].volume;
        float startPan    = state.tracks[trackIndex].pan;
        float elapsed     = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);

            applyEntry(trackIndex,
                Mathf.Lerp(startVolume, targetVolume, t),
                Mathf.Lerp(startPan,    targetPan,    t));

            yield return null;
        }

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