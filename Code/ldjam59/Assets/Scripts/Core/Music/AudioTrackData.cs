using UnityEngine;

[CreateAssetMenu(fileName = "AudioTrackData", menuName = "Audio/Track Data")]
public class AudioTrackData : ScriptableObject
{
    public string trackName;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(-1f, 1f)] public float pan = 0f;
    public bool muted;
    public bool soloed;
    public bool loop = false;
}
