using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts.Core.Music
{
    public class VolumeBehaviour : MonoBehaviour
    {
        [SerializeField] AudioEngine audioEngine;

        private void Awake()
        {
            if (audioEngine != null)
            {
                GameFrame.Base.Audio.Background.VolumeChanged.AddListener((volume) => audioEngine.SetMasterVolume(volume));
            }
        }
    }
}
