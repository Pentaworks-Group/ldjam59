using UnityEngine;

/// <summary>
/// Public API for the entire audio Engine. Game code should only need to touch this class.
/// 
/// -- Subsystems --
/// MultitrackPlayer        - plays/pauses/seeks the audio tracks
/// CombinationManager      - switches which tracks are audible and at what volume/pan
/// BeatClock               - derives bar/beat from playback position
/// BarCombinationTrigger   - queues combination changes to fire on a bar boundary
/// TrackPanner             - pans tracks based on a world-space objects's screen position
/// DistortionEffect        - applies radio / signal-loss effect to tracks
/// </summary>
public class AudioEngine : MonoBehaviour
{
    // -----------------------------------------------------------------
    // Subsystem references - wire in Inspector
    // -----------------------------------------------------------------

    [Header("Subsystems")]
    public MultitrackPlayer           player;
    public CombinationManager         combinations;
    public BeatClock                  beatClock;
    public BarCombinationTrigger      barTrigger;
    public TrackPanner                panner;
    public DistortionEffect           distortion;
    public RandomCombinationScheduler randomScheduler;

    // -----------------------------------------------------------------
    // Convenience read-only properties
    // -----------------------------------------------------------------

    public bool  IsPlaying           => player.state.isPlaying;
    public float Position            => player.GetPosition();
    public float Duration            => player.GetDuration();
    public int   CurrentBar          => beatClock.CurrentBar;
    public int   CurrentBeat         => beatClock.CurrentBeat;
    public bool  HasPendingCue       => barTrigger.HasPendingChange;
    public int   BarsUntilCue        => barTrigger.BarsUntilChange;
    public bool  IsRadioEffectActive => distortion.IsEffectActive;
    public float MasterVolume        => player.GetMasterVolume();

    // -----------------------------------------------------------------
    // Playback
    // -----------------------------------------------------------------

    public void Play()                   => player.Play();
    public void Pause()                  => player.Pause();
    public void Stop()                   => player.Stop();
    public void Seek(float seconds)      => player.Seek(seconds);
    public void SetMasterVolume(float v) => player.SetMasterVolume(v);

    // -----------------------------------------------------------------
    // Combinations - immediate
    // -----------------------------------------------------------------

    /// <summary>Switch combination instantly.</summary>
    public void SetCombination(string name)
        => combinations.ActivateCombination(name);

    /// <summary>Switch combination with a volume/pan fade.</summary>
    public void SetCombination(string name, float fadeDuration)
        => combinations.ActivateCombinationFaded(name, fadeDuration);

    /// <summary>Fade all tracks out.</summary>
    public void FadeOutAll(float fadeDuration)
        => combinations.FadeOutAll(fadeDuration);

    // -----------------------------------------------------------------
    // Combinations - bar-synced
    // -----------------------------------------------------------------

    /// <summary>
    /// Queue a combination to activate at the start of the next bar.
    /// The most common call - use this whenever you want a musically clean transition.
    /// </summary>
    public void CueOnNextBar(string name, float fadeDuration = -1)
        => barTrigger.QueueOnNextBar(name, fadeDuration);

    /// <summary>QUeue a combination to activate at a specific bar number.</summary>
    public void CueAtBar(int bar, string name, float fadeDuration = -1)
        => barTrigger.QueueAtBar(bar, name, fadeDuration);

    /// <summary>Cancel any pending bar-synced cue. </summary>
    public void CancelCue() => barTrigger.CancelQueue();

    // -----------------------------------------------------------------
    // Spatial panning
    // -----------------------------------------------------------------

    /// <summary>
    /// Track a world-space object and pan the given tracks based on its screen position.
    /// Pass null as target to stop tracking and leave pan at its current value.
    /// </summary>
    public void TrackObject(Transform target, int[] trackIndices = null)
    {
        panner.SetTarget(target);
        if (trackIndices != null)
            panner.SetTracksToAffect(trackIndices);
    }

    /// <summary>Stop spatial tracking. </summary>
    public void StopTracking() => panner.SetTarget(null);

    // -----------------------------------------------------------------
    // Distortion / radio effect
    // -----------------------------------------------------------------

    /// <summary>Apply radio distortion to all tracks. </summary>
    public void EnableRadioEffect(bool fade = true, float duration = -1f)
        => distortion.EnableRadioEffect(fade, duration);

    /// <summary>Appl radio distortion to specific tracks only. </summary>
    public void EnableRadioEffect(int[] trackIndices, bool fade = true, float duration = -1f)
        => distortion.EnableRadioEffect(trackIndices, fade, duration);

    /// <summary>Remove radio distortion. </summary>
    public void DisableRadioEffect(bool fade = true, float duration = -1f)
        => distortion.DisableRadioEffect(fade, duration);

    /// <summary>Trigger a temporary signal-loss effect (fade-in, stutter, fade-out) </summary>
    /// <param name="duration"></param>
    public void TriggerSignalLoss(float duration)
        => distortion.TriggerSignalLoss(duration);

    // -----------------------------------------------------------------
    // Random combination cycling
    // -----------------------------------------------------------------

    /// <summary>Start switching randomly between the given combinations,
    /// waiting between minBarInterval and maxBarInterval bars between each switch. </summary>
    public void StartRandomCycling(string[] combinations, int minBarInterval, int maxBarInterval, float fadeDuration = -1f)
        => randomScheduler.StartRandomCycling(combinations, minBarInterval, maxBarInterval, fadeDuration);

    /// <summary>Stop random cycling. Cancels any pending cue. </summary>
    public void StopRandomCycling()
        => randomScheduler.StopRandomCycling();

    // -----------------------------------------------------------------
    // Compound helpers - common multi-step patterns
    // -----------------------------------------------------------------

    /// <summary>
    /// Transition to a new combination at the next bar and simultaneously
    /// start tracking an object for spatial panning on the given tracks.
    /// Useful when a new character or object becomes the audio focus.
    /// </summary>
    public void FocusObject(Transform target, string combination, int[] panTrack, float fadeDuration = -1f)
    {
        CueOnNextBar(combination, fadeDuration);
        TrackObject(target, panTrack);
    }

    /// <summary>
    /// Queue a combination change at the next bar and apply a radio effect
    /// to specific tracks.
    /// </summary>
    /// <param name="combination"></param>
    /// <param name="radioTracks"></param>
    /// <param name="fadeDuration"></param>
    public void CueRadioTransition(string combination, int[] radioTracks, float fadeDuration = -1f)
    {
        CueOnNextBar(combination, fadeDuration);
        EnableRadioEffect(radioTracks);
    }

    /// <summary>
    /// Hard reset: stop playback, clear any pending cue, stop tracking,
    /// disable all effects. Useful on scene unload or game-over.
    /// </summary>
    public void ResetAll()
    {
        player.Stop();
        combinations.FadeOutAll(0f);
        barTrigger.CancelQueue();
        panner.SetTarget(null);
        distortion.DisableRadioEffect(false);
    }
}
