using UnityEngine;

public class TrackPanner : MonoBehaviour
{
    [Header("References")]
    public MultitrackPlayer player;
    public MultitrackPlayerState state;
    public Camera trackingCamera;       // Leave null to user Camera.main

    [Header("Target")]
    public Transform target;            // The object to track

    [Header("Track Selection")]
    [Tooltip("Indices of tracks that should receive the panning.")]
    public int[] tracksToAffect = new int[0];

    [Header("Pan Settigns")]
    [Range(0f, 1f)]
    [Tooltip("Width of the dead zone at screen center (0 = pan starts immediately).")]
    public float deadZone = 0.05f;

    [Tooltip("Pan range: 1 = full -1..+1, 0.5 = only reaches -0.5..+0.5")]
    [Range(0f, 1f)]
    public float panRange = 1f;

    [Tooltip("Smoothing time in seconds (0 = no smoothing).")]
    public float smoothTime = 0.1f;

    [Header("Debug")]
    public bool showDebugLog = false;

    private float currentPan = 0f;
    private float panVelocity = 0f;

    public void Update()
    {
        if (target == null || player == null || state == null) return;

        float targetPan = computeTargetPan();

        currentPan = smoothTime > 0f
            ? Mathf.SmoothDamp(currentPan, targetPan, ref panVelocity, smoothTime)
            : targetPan;

        applyPan(currentPan);

        if (showDebugLog)
            Debug.Log($"[TrackPanner] targetPan={targetPan:F3} currentPan{currentPan:F3}");
    }

    private float computeTargetPan()
    {
        Camera cam = trackingCamera != null ? trackingCamera : Camera.main;
        if (cam == null) return 0f;

        // viewport X is 0 (left) .. 1 (right); remap to -1..+1
        Vector3 viewport = cam.WorldToViewportPoint(target.position);
        float rawPan = (viewport.x - 0.5f) * 2f; // -1..+1

        // Dead zone: flatten values near centre to zero
        float sign = Mathf.Sign(rawPan);
        float abs = Mathf.Abs(rawPan);
        float adjusted = abs < deadZone ? 0f : (abs - deadZone) / (1f - deadZone);

        return Mathf.Clamp(sign * adjusted * panRange, -1f, 1f);
    }

    private void applyPan(float pan)
    {
        foreach (int index in tracksToAffect)
        {
            if (index < 0 || index >= tracksToAffect.Length) continue;

            state.tracks[index].pan = pan;

            var src = player.GetSource(index);
            if (src != null) src.panStereo = pan;
        }
    }

    public void SetTarget(Transform newTarget) => target = newTarget;

    public void SetTracksToAffect(int[] indices) => tracksToAffect = indices;

    public void ResetPan() => target = null;
}
