using UnityEngine;
using System.Runtime.InteropServices;
public class WebAudioBridge
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    public static extern void ResumeAudioContext();
#else
    public static void ResumeAudioContext() { }
#endif   
}
