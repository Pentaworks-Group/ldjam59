using UnityEngine;

namespace Assets.Scripts.Base
{
    public static class Audio
    {
        private static AudioEngine audioEngine;
        public static AudioEngine AudioEngine
        {
            get
            {
                if (audioEngine == default)
                {
                    var prefab = UnityEngine.Resources.Load<GameObject>("Prefabs/Audio/AudioSystem");

                    if (prefab != default)
                    {
                        var audioSystem = GameObject.Instantiate(prefab);

                        GameObject.DontDestroyOnLoad(audioSystem);

                        if (audioSystem.TryGetComponent<AudioEngine>(out var loadedEngine))
                        {
                            audioEngine = loadedEngine;
                        }
                    }
                }

                return audioEngine;
            }
        }
    }
}
