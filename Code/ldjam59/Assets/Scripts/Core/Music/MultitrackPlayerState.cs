using UnityEngine;

[CreateAssetMenu(fileName = "MultitrackPlayerState", menuName = "Audio/Player State")]
public class MultitrackPlayerState : ScriptableObject
{
    public AudioTrackData[] tracks;
    public float currentPosition = 0f;
    public bool isPlaying = false;
    public bool loopAll = false;
}
