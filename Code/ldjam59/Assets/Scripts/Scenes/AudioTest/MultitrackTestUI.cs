using UnityEngine;

public class MultitrackTestUI : MonoBehaviour
{
    public MultitrackPlayer player;

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(20, 20, 300, 400));

        GUILayout.Label($"Position: {player.GetPosition():F1}s / {player.GetDuration():F1}s");
        GUILayout.Label($"Spielt: {player.state.isPlaying}");

        GUILayout.Space(10);

        if (GUILayout.Button("Play", GUILayout.Height(40))) player.Play();
        if (GUILayout.Button("Pause", GUILayout.Height(40))) player.Pause();
        if (GUILayout.Button("Stop", GUILayout.Height(40))) player.Stop();

        GUILayout.Space(10);

        bool loopAll = player.state.loopAll;
        bool newLoop = GUILayout.Toggle(loopAll, "Alle Spuren loopen");
        if (newLoop != loopAll)
            player.SetLoopAll(newLoop);

        for (int i = 0; i < player.state.tracks.Length; i++)
        {
            var track = player.state.tracks[i];
            GUILayout.BeginHorizontal();
            GUILayout.Label(track.trackName, GUILayout.Width(80));
            track.volume = GUILayout.HorizontalSlider(track.volume, 0f, 1f);
            player.ApplyTrackSettings(i);
            if (GUILayout.Button(track.muted ? "mute" :  "unmute", GUILayout.Width(30)))
            {
                track.muted = !track.muted;
                player.ApplyTrackSettings(i);
            }

            bool trackLoop = track.loop;
            bool newTrackLoop = GUILayout.Toggle(trackLoop, "L", GUILayout.Width(30));
            if (newTrackLoop != trackLoop)
                player.SetTrackLoop(i, newTrackLoop);
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(10);

        GUILayout.EndArea();
    }
}
