using UnityEngine;

[CreateAssetMenu(fileName = "TrackCombination", menuName = "Audio/Track Combination")]
public class TrackCombination : ScriptableObject
{
    public string combinationName;
    public TrackCombinationEntry[] tracks;
}

[System.Serializable]
public class TrackCombinationEntry
{
    public bool active = true;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(-1f, 1f)] public float pan = 0f;
}