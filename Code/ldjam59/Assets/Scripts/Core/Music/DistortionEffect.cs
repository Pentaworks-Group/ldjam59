using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

public class DistortionEffect : MonoBehaviour
{
    [Header("References")]
    public MultitrackPlayer player;
    public MultitrackPlayerState state;
    public AudioMixer mixer;

    [Header("Mixer Parameter Names")]
    // These must match the exposed parameter names in the AudioMixer exactly
    public string[] lowCutParams;
    public string[] highCutParams;
    public string[] distortionParams;
    public string[] volumeParams;

    [Header("Effect Settings")]
    [Range(0f, 1f)] public float intensity = 1f;        // overall strength
    [Range(0f, 1f)] public float crackleRate = 0.3f;    // how often crackle hits
    public float fadeDuration = 1.5f;                   // in/out duration in seconds

    // Neutral mixer values
    private const float NEUTRAL_LOWCUT = 20f;      // Hz
    private const float NEUTRAL_HIGHCUT = 20000f;   // Hz
    private const float NEUTRAL_DISTORT = 0f;
    private const float NEUTRAL_VOLUME_DB = 1.0f;

    // Radio effect target values
    private const float RADIO_LOWCUT = 500f;
    private const float RADIO_HIGHCUT = 3500f;
    private const float RADIO_DISTORT = 0.6f;

    private bool effectActive = false;
    private bool[] trackAffected;
    private Coroutine crackleCoroutine;
    private List<Coroutine> fadeCoroutines = new List<Coroutine>();

    void Awake()
    {
        trackAffected = new bool[state.tracks.Length];
    }

    public void EnableRadioEffect(bool fade = true)
    {
        for (int i = 0; i < state.tracks.Length; i++)
            trackAffected[i] = true;
        applyEffect(fade);
    }

    public void EnableRadioEffect(int[] trackIndices, bool fade = true)
    {
        for (int i = 0; i < trackAffected.Length; i++)
            trackAffected[i] = false;

        foreach (int idx in trackIndices)
            if (idx < trackAffected.Length)
                trackAffected[idx] = true;
        applyEffect(fade);
    }

    public void DisableRadioEffect(bool fade = true)
    {
        stopAllFades();
        if (crackleCoroutine != null)
        {
            StopCoroutine(crackleCoroutine);
            crackleCoroutine = null;
        }
        effectActive = false;

        for (int i = 0; i < state.tracks.Length; i++)
        {
            if (!trackAffected[i]) continue;
            if (fade)
                fadeCoroutines.Add(StartCoroutine(
                    fadeParams(i, getCurrentParams(i), getNeutralParams(), fadeDuration)
                ));
            else
                setParams(i, getNeutralParams());
        }
    }

    public void TriggerSignalLoss(float duration)
    {
        StartCoroutine(signalLossSequence(duration));
    }

    private void applyEffect(bool fade)
    {
        stopAllFades();
        effectActive = true;

        var target = getRadioParams();

        for (int i = 0; i < state.tracks.Length; i++)
        {
            if (!trackAffected[i]) continue;
            if (fade)
                fadeCoroutines.Add(StartCoroutine(
                    fadeParams(i, getCurrentParams(i), target, fadeDuration)
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
            highCut = Mathf.Lerp(RADIO_HIGHCUT, 2000f, intensity),
            distort = Mathf.Lerp(RADIO_DISTORT, 1f, intensity),
            volumeDB = -6f * intensity,
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
            float drop = Random.Range(-20f, - 3f) * intensity;
            for (int i = 0; i < state.tracks.Length; i++)
            {
                if (!trackAffected[i]) continue;
                mixer.SetFloat(volumeParams[i], drop);
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
            float wait = Random.Range(0.1f, 2f / (crackleRate + 0.01f));
            yield return new WaitForSeconds(wait);

            if (!effectActive ) yield break;

            // Short volume dip on affected tracks
            float dip = Random.Range(-15f, -2f) * intensity;
            float dipTime = Random.Range(0.02f, 0.12f);

            for (int i = 0;i < state.tracks.Length;i++)
            {
                if (!trackAffected[i]) continue;
                if (i < volumeParams.Length)
                    mixer.SetFloat(volumeParams[i], dip);
            }

            yield return new WaitForSeconds(dipTime);

            // Restore volume
            for (int i = 0; i < state.tracks.Length;i++)
            {
                if (!trackAffected[i]) continue;
                if (i < volumeParams.Length)
                    mixer.SetFloat(volumeParams[i], NEUTRAL_VOLUME_DB);
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
                highCut = Mathf.Lerp(from.highCut, to.highCut, t),
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
        if (index < highCutParams.Length) mixer.SetFloat(highCutParams[index], p.highCut);
        if (index < distortionParams.Length) mixer.SetFloat(distortionParams[index], p.distort);
        if (index < volumeParams.Length) mixer.SetFloat(volumeParams[index], p.volumeDB);
    }

    private MixerParams getCurrentParams(int index)
    {
        var p = new MixerParams();
        if (index < lowCutParams.Length) mixer.GetFloat(lowCutParams[index], out p.lowCut);
        if (index < highCutParams.Length) mixer.GetFloat(highCutParams[index], out p.highCut);
        if (index < distortionParams.Length) mixer.GetFloat(distortionParams[index], out p.distort);
        if (index < volumeParams.Length) mixer.GetFloat(volumeParams[index], out p.volumeDB);
        return p;
    }

    private MixerParams getRadioParams() => new MixerParams
    {
        lowCut = Mathf.Lerp(NEUTRAL_LOWCUT, RADIO_LOWCUT, intensity),
        highCut = Mathf.Lerp(NEUTRAL_HIGHCUT, RADIO_HIGHCUT, intensity),
        distort = Mathf.Lerp(NEUTRAL_DISTORT, RADIO_DISTORT, intensity),
        volumeDB = NEUTRAL_VOLUME_DB
    };

    private MixerParams getNeutralParams() => new MixerParams
    {
        lowCut = NEUTRAL_LOWCUT,
        highCut = NEUTRAL_HIGHCUT,
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
        public float highCut;
        public float distort;
        public float volumeDB;
    }

}
