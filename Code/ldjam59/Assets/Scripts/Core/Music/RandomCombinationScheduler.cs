using UnityEngine;

public class RandomCombinationScheduler : MonoBehaviour
{
    [Header("References")]
    public BarCombinationTrigger barTrigger;
    public BeatClock beatClock;

    // Runtime state
    private string[] activeCombinationSet;
    private int minBars;
    private int maxBars;
    private float fadeDuration;
    private string lastCombination;
    private bool running;

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

    /// <summary>
    /// Start randomly cycling through the given combinations.
    /// </summary>
    /// <param name="combinations">Set of combination names to pick from.</param>
    /// <param name="minBarInterval">Minimum bars between switches.</param>
    /// <param name="maxBarInterval">Maximum bars between switches.</param>
    /// <param name="fadeDuration">Fade duration per switch. -1 uses BarCombinationTrigger default.</param>
    public void StartRandomCycling(string[] combinations, int minBarInterval, int maxBarInterval, float fadeDuration = -1f)
    {
        if (combinations == null || combinations.Length == 0)
        {
            Debug.LogWarning("[RandomCombinationScheduler] Cannot start: combination set is empty.");
            return;
        }

        if (minBarInterval < 1) minBarInterval = 1;
        if (maxBarInterval < minBarInterval) maxBarInterval = minBarInterval;

        activeCombinationSet = combinations;
        minBars = minBarInterval;
        maxBars = maxBarInterval;
        this.fadeDuration = fadeDuration;
        lastCombination = null;
        running = true;

        scheduleNext();
    }

    /// <summary>Stop the random cycling. Any already-queued cue is also cancelled. </summary>
    public void StopRandomCycling()
    {
        running = false;
        barTrigger.CancelQueue();
    }

    public bool IsRunning() => running;

    private void onBarChanged(int bar)
    {
        if (!running) return;

        // The barTrigger has just fired (its pending cue cleared) - schedule the next one.
        // We detect this by checking: we were expecting a cue and it's now gone.
        if (!barTrigger.HasPendingChange)
            scheduleNext();
    }

    private void scheduleNext()
    {
        if (!running) return;

        string next = pickRandom();
        int barsFromNow = Random.Range(minBars, maxBars + 1);
        int targetBar = beatClock.CurrentBar + barsFromNow;

        barTrigger.QueueAtBar(targetBar, next, fadeDuration);
    }

    private string pickRandom()
    {
        if (activeCombinationSet.Length == 1) return activeCombinationSet[0];

        // Avoid repeating the same combination twice in a row
        string pick;
        int attemps = 0;
        do
        {
            pick = activeCombinationSet[Random.Range(0, activeCombinationSet.Length)];
            attemps++;
        }
        while (pick == lastCombination && attemps < 10);

        lastCombination = pick;
        return pick;
    }
}
