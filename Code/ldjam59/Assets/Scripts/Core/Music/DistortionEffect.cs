using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using static UnityEngine.UI.CanvasScaler;

public class DistortionEffect : MonoBehaviour
{
    [Header("References")]
    public MultitrackPlayer player;
    public MultitrackPlayerState state;
    public AudioMixer mixer;

    [Header("Mixer Parameter Names")]
    // These must match the exposed parameter names in the AudioMixer exactly
    public string[] lowCutParams;
    public string[] lowCutVolumeParams;
    public string[] highCutParams;
    public string[] highCutVolumeParams;
    public string[] distortionParams;
    public string[] volumeParams;

    [Header("Effect Settings")]
    [Range(0f, 1f)] public float intensity = 1f;        // overall strength
    [Range(0f, 1f)] public float crackleRate = 0.5f;    // how often crackle hits
    public float fadeDuration = 1.5f;                   // in/out duration in seconds

    // Neutral mixer values
    private const float NEUTRAL_LOWCUT = 20f;      // Hz
    private const float NEUTRAL_LOWCUT_VOLUME = 1f;
    private const float NEUTRAL_HIGHCUT = 20000f;   // Hz
    private const float NEUTRAL_HIGHCUT_VOLUME = 1f;
    private const float NEUTRAL_DISTORT = 0f;
    private const float NEUTRAL_VOLUME_DB = 0.0f;

    // Radio effect target values
    private const float RADIO_LOWCUT = 500f;
    private const float RADIO_LOWCUT_VOLUME = 0.05f;
    private const float RADIO_HIGHCUT = 3500f;
    private const float RADIO_HIGHCUT_VOLUME = 0.05f;
    private const float RADIO_DISTORT = 1.0f;

    private bool effectActive = false;
    private bool[] trackAffected;
    private Coroutine crackleCoroutine;
    private List<Coroutine> fadeCoroutines = new List<Coroutine>();

    // Cached per-track mixer state
    private MixerParams[] cachedParams;

    void Awake()
    {
        Assets.Scripts.Base.Core.Game.ExecuteAfterInstantation(Init);
    }

    private void Init()
    {
        trackAffected = new bool[state.tracks.Length];

        // Initialize cache to neutral so fades always start from a valid value,
        // even before the effect has been applied for the first time.
        cachedParams = new MixerParams[state.tracks.Length];
        for (int i = 0; i < cachedParams.Length; i++)
            cachedParams[i] = getNeutralParams();
    }

    public void EnableRadioEffect(bool fade = true, float duration = -1f)
    {
        for (int i = 0; i < state.tracks.Length; i++)
            trackAffected[i] = true;
        applyEffect(fade, duration);
    }

    public void EnableRadioEffect(int[] trackIndices, bool fade = true, float duration = -1f)
    {
        for (int i = 0; i < trackAffected.Length; i++)
            trackAffected[i] = false;

        foreach (int idx in trackIndices)
            if (idx < trackAffected.Length)
                trackAffected[idx] = true;

        applyEffect(fade, duration);
    }

    public void DisableRadioEffect(bool fade = true, float duration = -1f)
    {
        stopAllFades(); 
        if (crackleCoroutine != null)
        {
            StopCoroutine(crackleCoroutine);
            crackleCoroutine = null;
        }
        effectActive = false;

        float resolvedDuration = (fade && duration >= 0f) ? duration : fadeDuration;

        for (int i = 0; i < state.tracks.Length; i++)
        {
            if (!trackAffected[i]) continue;
            if (fade)
                fadeCoroutines.Add(StartCoroutine(
                    fadeParams(i, getCurrentParams(i), getNeutralParams(), resolvedDuration)
                ));
            else
                setParams(i, getNeutralParams());
        }
    }

    public void TriggerSignalLoss(float duration)
    {
        StartCoroutine(signalLossSequence(duration));
    }

    public bool IsEffectActive => effectActive;

    private void applyEffect(bool fade, float duration = -1f)
    {
        stopAllFades();
        effectActive = true;

        float resolvedDuration = (fade && duration >= 0f) ? duration : fadeDuration;
        var target = getRadioParams();

        for (int i = 0; i < state.tracks.Length; i++)
        {
            if (!trackAffected[i]) continue;
            if (fade)
                fadeCoroutines.Add(StartCoroutine(
                    fadeParams(i, getCurrentParams(i), target, resolvedDuration)
                ));
            else
                setParams(i, target);
        }

        if (crackleCoroutine != null) StopCoroutine(crackleCoroutine);
        crackleCoroutine = StartCoroutine(crackleLoop());
    }

    private IEnumerator signalLossSequence(float duration)
    {
        effectActive = true;
        float half = duration * 0.5f;

        // Phase 1: fade into heavy radio effect
        var heavy = new MixerParams
        {
            lowCut = Mathf.Lerp(RADIO_LOWCUT, 1200f, intensity),
            lowCutVolume = Mathf.Lerp(RADIO_LOWCUT_VOLUME, 1f, intensity),
            highCut = Mathf.Lerp(RADIO_HIGHCUT, 2000f, intensity),
            highCutVolume = Mathf.Lerp(RADIO_HIGHCUT_VOLUME, 1f, intensity),
            distort = Mathf.Lerp(RADIO_DISTORT, 0.99f, intensity),
            volumeDB = -20f * intensity,
        };

        for (int i = 0; i < state.tracks.Length; i++)
        {
            if (!trackAffected[i]) continue;
            StartCoroutine(fadeParams(i, getCurrentParams(i), heavy, half * 0.4f));
        }

        yield return new WaitForSeconds(half * 0.4f);

        // Phase 2: stutter - random volume drops
        float elapsed = 0f;
        while (elapsed < half)
        {
            float drop = Random.Range(-40f, - 3f) * intensity;
            for (int i = 0; i < state.tracks.Length; i++)
            {
                if (!trackAffected[i]) continue;
                var p = getCurrentParams(i);
                p.volumeDB = drop;
                setParams(i, p);
            }
            float wait = Random.Range(0.05f, 0.2f);
            yield return new WaitForSeconds(wait);
            elapsed += wait;
        }

        // Phase 3: fade back to normal radio sound
        var radioParams = getRadioParams();
        for (int i = 0; i < state.tracks.Length; i++)
        {
            if (!trackAffected[i]) continue;
            StartCoroutine(fadeParams(i, getCurrentParams(i), radioParams, half * 0.6f));
        }
    }

    private IEnumerator crackleLoop()
    {
        while (effectActive)
        {
            float wait = Random.Range(0.01f, 0.4f / (crackleRate + 0.01f));
            yield return new WaitForSeconds(wait);

            if (!effectActive ) yield break;

            // Short volume dip on affected tracks
            float dip = Random.Range(-40f, -2f) * intensity;
            float dipTime = Random.Range(0.1f, 0.5f);

            for (int i = 0;i < state.tracks.Length;i++)
            {
                if (!trackAffected[i]) continue;
                var p = getCurrentParams(i);
                p.volumeDB = dip;
                setParams(i, p);
            }

            yield return new WaitForSeconds(dipTime);

            // Restore volume to wahtever the cache says the non-dipped value should be.
            for (int i = 0; i < state.tracks.Length;i++)
            {
                if (!trackAffected[i]) continue;
                var p = getCurrentParams(i);
                p.volumeDB = dip;
                setParams(i, p);
            }
        }
    }

    private IEnumerator fadeParams(int trackIndex, MixerParams from, MixerParams to, float duration)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);

            setParams(trackIndex, new MixerParams
            {
                lowCut = Mathf.Lerp(from.lowCut, to.lowCut, t),
                lowCutVolume = Mathf.Lerp(from.lowCutVolume, to.lowCutVolume, t),
                highCut = Mathf.Lerp(from.highCut, to.highCut, t),
                highCutVolume = Mathf.Lerp(from.highCutVolume, to.highCutVolume, t),
                distort = Mathf.Lerp(from.distort, to.distort, t),
                volumeDB = Mathf.Lerp(from.volumeDB, to.volumeDB, t),
            });

            yield return null;
        }
        setParams(trackIndex, to);
    }

    private void setParams(int index, MixerParams p)
    {
        if (index < lowCutParams.Length) mixer.SetFloat(lowCutParams[index], p.lowCut);
        if (index < lowCutVolumeParams.Length) mixer.SetFloat(lowCutVolumeParams[index], p.lowCutVolume);
        if (index < highCutParams.Length) mixer.SetFloat(highCutParams[index], p.highCut);
        if (index < highCutVolumeParams.Length) mixer.SetFloat(highCutVolumeParams[index], p.highCutVolume);
        if (index < distortionParams.Length) mixer.SetFloat(distortionParams[index], p.distort);
        if (index < volumeParams.Length) mixer.SetFloat(volumeParams[index], p.volumeDB);

        cachedParams[index] = p;
    }

    private MixerParams getCurrentParams(int index) => cachedParams[index];

    private MixerParams getRadioParams() => new MixerParams
    {
        lowCut = Mathf.Lerp(NEUTRAL_LOWCUT, RADIO_LOWCUT, intensity),
        lowCutVolume = Mathf.Lerp(NEUTRAL_LOWCUT_VOLUME, RADIO_LOWCUT_VOLUME, intensity),
        highCut = Mathf.Lerp(NEUTRAL_HIGHCUT, RADIO_HIGHCUT, intensity),
        highCutVolume = Mathf.Lerp(NEUTRAL_HIGHCUT_VOLUME, RADIO_HIGHCUT_VOLUME, intensity),
        distort = Mathf.Lerp(NEUTRAL_DISTORT, RADIO_DISTORT, intensity),
        volumeDB = NEUTRAL_VOLUME_DB
    };

    private MixerParams getNeutralParams() => new MixerParams
    {
        lowCut = NEUTRAL_LOWCUT,
        lowCutVolume = NEUTRAL_LOWCUT_VOLUME,
        highCut = NEUTRAL_HIGHCUT,
        highCutVolume = NEUTRAL_HIGHCUT_VOLUME,
        distort = NEUTRAL_DISTORT,
        volumeDB = NEUTRAL_VOLUME_DB
    };

    private void stopAllFades()
    {
        foreach (var c in fadeCoroutines)
            if (c != null) StopCoroutine(c);
        fadeCoroutines.Clear();
    }

    private struct MixerParams
    {
        public float lowCut;
        public float lowCutVolume;
        public float highCut;
        public float highCutVolume;
        public float distort;
        public float volumeDB;
    }

}
