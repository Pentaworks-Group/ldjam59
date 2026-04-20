using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MultitrackPlayer : MonoBehaviour, IPointerClickHandler
{
    [Header("Shared State (ScriptableObject)")]
    public MultitrackPlayerState state;

    [Header("Mixer Groups")]
    public AudioMixerGroup[] mixerGroups;

    private AudioSource[] sources;
    private bool isPlaying = false;

    private bool started = false;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (GameFrame.Base.Audio.Background != default)
        {
            if (state.masterVolume != GameFrame.Base.Audio.Background.Volume)
            {
                state.masterVolume = GameFrame.Base.Audio.Background.Volume;
            }
        }

        InitializeSources();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Restore position from state
        if (state.isPlaying)
        {
            Seek(state.currentPosition);
            Play();
        }
    }

    void InitializeSources()
    {
        // Cleanup old AudioSources
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        sources = new AudioSource[state.tracks.Length];

        for (int i = 0; i < state.tracks.Length; i++)
        {
            var trackData = state.tracks[i];
            var go = new GameObject($"Track_{trackData.trackName}");
            go.transform.parent = transform;

            sources[i] = go.AddComponent<AudioSource>();
            sources[i].clip = trackData.clip;
            sources[i].playOnAwake = false;
            sources[i].loop = trackData.loop || state.loopAll;

            // Assign mixer group if available for this track index
            if (mixerGroups != null && i < mixerGroups.Length && mixerGroups[i] != null)
                sources[i].outputAudioMixerGroup = mixerGroups[i];

            ApplyTrackSettings(i);
        }
    }

    public void Play()
    {
        if (sources == null)
        {
            Debug.LogWarning("Can't Play! no sources provided.");
            return;
        }

        if (isPlaying)
        {
            return;
        }

        WebAudioBridge.ResumeAudioContext();

        isPlaying = true;
        state.isPlaying = true;

        double dspStart = AudioSettings.dspTime + 0.1;

        for (int i = 0; i < sources.Length; i++)
        {
            ApplyTrackSettings(i);
            sources[i].PlayScheduled(dspStart);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (started) return;
        started = true;

        WebAudioBridge.ResumeAudioContext();
        Play();
    }

    public void Pause()
    {
        state.currentPosition = GetPosition();
        state.isPlaying = false;
        isPlaying = false;

        foreach (var src in sources)
            src.Pause();
    }

    public void Stop()
    {
        state.currentPosition = 0f;
        state.isPlaying = false;
        isPlaying = false;

        foreach (var src in sources)
        {
            src.Stop();
            src.time = 0f;
        }
    }

    public void Seek(float seconds)
    {
        state.currentPosition = seconds;
        foreach (var src in sources)
            src.time = seconds;
    }

    public void SetLoopAll(bool loop)
    {
        state.loopAll = loop;
        for (int i = 0; i < sources.Length; i++)
            sources[i].loop = loop || state.tracks[i].loop;
    }

    public void SetTrackLoop(int index, bool loop)
    {
        state.tracks[index].loop = loop;
        sources[index].loop = loop || state.loopAll;
    }

    public void ApplyTrackSettings(int index)
    {
        var data = state.tracks[index];
        var src = sources[index];

        float effectiveVolume = data.muted ? 0f : data.volume * state.masterVolume;
        src.volume = effectiveVolume;
        src.panStereo = data.pan;
        src.loop = data.loop || state.loopAll;
    }

    public void SetMasterVolume(float volume)
    {
        state.masterVolume = Mathf.Clamp01(volume);
        for (int i = 0; i < sources.Length; i++)
            ApplyTrackSettings(i);
    }

    public float GetMasterVolume() => state.masterVolume;

    public void SetSolo(int index, bool solo)
    {
        state.tracks[index].soloed = solo;
        bool anySolo = System.Array.Exists(state.tracks, t => t.soloed);

        for (int i = 0; i < sources.Length; i++)
        {
            var data = state.tracks[i];
            float effectiveVolume = anySolo
                ? (data.soloed ? data.volume * state.masterVolume : 0f)
                : (data.muted ? 0f : data.volume * state.masterVolume);
            sources[i].volume = effectiveVolume;
        }
    }

    public float GetPosition() =>
        sources.Length > 0 ? sources[0].time : 0f;

    public float GetDuration()
    {
        float max = 0f;
        foreach (var t in state.tracks)
            if (t.clip != null && t.clip.length > max)
                max = t.clip.length;
        return max;
    }

    public AudioSource GetSource(int index)
    {
        if (index < 0 || index >= sources.Length) return null;
        return sources[index];
    }

    // Switch Tracks during runtime (e.g. Scene change)
    public void LoadTracks(AudioTrackData[] newTracks)
    {
        state.tracks = newTracks;
        state.currentPosition = 0f;
        state.isPlaying = false;
        isPlaying = false;
        InitializeSources();
    }

    private void Update()
    {
        // sync state
        if (isPlaying)
        {
            state.currentPosition = GetPosition();
        }
    }
}
