using UnityEngine;

public class WebAudioBridge
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    public static extern void ResumeAudioContext();
#else
    public static void ResumeAudioContext() { }
#endif   
}
