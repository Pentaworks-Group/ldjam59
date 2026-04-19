using System;
using UnityEngine;

/// <summary>
/// Tracks musical time based on the MultitrackPlayer position
/// Fires OnBarChanged and OnBeatChanged events that other systems can subscribe to.
///
/// Assumes constant tempo and meter throughout.
/// </summary>
public class BeatClock : MonoBehaviour
{
    [Header("References")]
    public MultitrackPlayer player;
    public MultitrackPlayerState state;

    [Header("Tempo & Meter")]
    [Tooltip("Beats per minute.")]
    public float bpm = 120f;

    [Tooltip("Number of beats per bar (numerator). Supports odd meters.")]
    public int beatsPerBar = 4;

    [Tooltip("Beat unit (denominator): 4 = quarter note, 8 = eighth note, etc.")]
    public int beatUnit = 4;

    [Tooltip("Audio position offset in seconds to correct any clip lead-in before bar 1.")]
    public float audioStartOffset = 0f;

    // One-based: bar 1, beat 1
    public int CurrentBar { get; private set; } = 1;
    public int CurrentBeat { get; private set; } = 1;

    // Fractional position within the current beat (0..1)
    public float BeatFraction { get; private set; } = 0f;

    public event Action<int> OnBarChanged;
    public event Action<int> OnBeatChanged;

    // Seconds per beat, accounting for beat unit relative to quarter note
    public float SecondsPerBeat => 60f / bpm * (4f / beatUnit);
    public float SecondsPerBar => SecondsPerBeat * beatsPerBar;

    private int lastBar = -1;
    private int lastBeat = -1;

    private void Update()
    {
        if (!state.isPlaying) return;

        float position = Mathf.Max(0f, state.currentPosition - audioStartOffset);

        float totalBeats = position / SecondsPerBeat;
        int barIndex = Mathf.FloorToInt(totalBeats / beatsPerBar); // 0-based
        int beatInBar = Mathf.FloorToInt(totalBeats % beatsPerBar); // 0-based
        BeatFraction = (totalBeats % 1f);

        CurrentBar = barIndex + 1;
        CurrentBeat = beatInBar + 1;

        if (CurrentBar != lastBar) 
        {
            lastBar = CurrentBar;
            OnBarChanged?.Invoke(CurrentBar);
        }

        if (CurrentBeat != lastBeat)
        {
            lastBeat = CurrentBeat;
            OnBeatChanged?.Invoke(CurrentBeat);
        }
    }

    public float GetBarTime(int bar) => audioStartOffset + (bar - 1) * SecondsPerBar;

    public float GetBeatTime(int bar, int beat) => audioStartOffset + ((bar - 1) * beatsPerBar + (beat - 1)) * SecondsPerBeat;
}
