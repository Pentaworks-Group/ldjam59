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
                    var prefab = UnityEngine.Resources.Load<GameObject>("AudioSystem");

                    if (prefab != default)
                    {
                        var gameObject = GameObject.Instantiate(prefab);

                        if (gameObject.TryGetComponent<AudioEngine>(out var loadedEngine))
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
