using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Queues a combination change to fire on a specific bar boundary,
/// driven by game events.
/// </summary>
public class BarCombinationTrigger : MonoBehaviour
{
    [Header("References")]
    public BeatClock beatClock;
    public CombinationManager combinationManager;

    [Header("Defaults")]
    [Tooltip("Fade duration used when none is specified in a Queue call.")]
    public float defaultFadeDuration = 0f;

    // -1 means no change queued
    private int pendingBar = -1;
    private int lastFiredBar = 0;
    private int accumulatedBars = 0;
    private string pendingCombinationName = null;
    private float pendingFadeDuration = 0f;

    private void OnEnable()
    {
        if (beatClock != null)
            beatClock.OnBarChanged += onBarChanged;
    }

    private void OnDisable()
    {
        if (beatClock != null)
            beatClock.OnBarChanged -= onBarChanged;
    }

    public void QueueOnNextBar(string combinationName, float fadeDuration = -1)
    {
        QueueAtBar(beatClock.CurrentBar + 1, combinationName, fadeDuration);
    }

    public void QueueAtBar(int bar, string combinationName, float fadeDuration = -1f)
    {
        if (bar <= beatClock.CurrentBar)
        {
            Debug.LogWarning($"[BarCombinationTrigger] Bar {bar} is already in the past " +
                             $"(current {beatClock.CurrentBar}). Queing for next bar instead.");
            bar = beatClock.CurrentBar + 1;
        }

        pendingBar = bar;
        pendingCombinationName = combinationName;
        pendingFadeDuration = fadeDuration >= 0f ? fadeDuration : defaultFadeDuration;

        Debug.Log($"[BarCombinationTrigger] Queued '{combinationName}' at bar {pendingBar}" +
                  $" (fade: {pendingFadeDuration}s)");
    }

    public void CancelQueue()
    {
        pendingBar = -1;
        pendingCombinationName = null;
        accumulatedBars = 0;
        lastFiredBar = 0;
    }

    public bool HasPendingChange => pendingBar >= 0;

    public int BarsUntilChange => HasPendingChange ? Mathf.Max(0, pendingBar - beatClock.CurrentBar) : 0;

    private void onBarChanged(int bar)
    {
        if (pendingBar < 0) return;

        // Track how many times we've looped by detecting backward jumps.
        if (bar < lastFiredBar)
            accumulatedBars += lastFiredBar;

        lastFiredBar = bar;

        int absoluteBar = accumulatedBars + bar;

        if (absoluteBar < pendingBar)
        {
            return;
        }

        if (pendingFadeDuration > 0f)
            combinationManager.ActivateCombinationFaded(pendingCombinationName, pendingFadeDuration);
        else
            combinationManager.ActivateCombination(pendingCombinationName);

        CancelQueue();
    }
}
