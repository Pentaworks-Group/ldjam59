using System;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Prefabs.Menu
{
	public class EmbeddedOptionsMenuBehaviour : MonoBehaviour
	{
		[SerializeField] Slider masterVolumeSlider;
		[SerializeField] Slider effectsVolumeSlider;

        private void Awake()
        {
            var playerOptions = Base.Core.Game.Options;

            if (masterVolumeSlider != null)
            {
                masterVolumeSlider.value = playerOptions.BackgroundVolume;
                masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
            }

            if (effectsVolumeSlider != null)
            {
                effectsVolumeSlider.value = playerOptions.EffectsVolume;
                effectsVolumeSlider.onValueChanged.AddListener(OnEffectsVolumeChanged);
            }
        }

        private void OnMasterVolumeChanged(Single value)
        {
            GameFrame.Base.Audio.Background.Volume = value;
        }

        private void OnEffectsVolumeChanged(Single value)
        {
            GameFrame.Base.Audio.Effects.Volume = value;
        }
    }
}
