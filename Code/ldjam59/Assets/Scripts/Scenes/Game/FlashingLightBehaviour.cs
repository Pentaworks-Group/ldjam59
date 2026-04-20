using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts.Scenes.Game
{
    [RequireComponent(typeof(Light))]
    public class FlashingLightBehaviour : MonoBehaviour
    {
        [SerializeField] private Light lightSource;
        [SerializeField] private Single interval = 0.5f;
        [SerializeField] private Single startDelay = 0f;
        [SerializeField] private Boolean isActive = false;

        private Single defaultIntensity;

        private void Start()
        {
            defaultIntensity = lightSource.intensity;

            if (!isActive)
            {
                lightSource.intensity = 0;
            }

            InvokeRepeating(nameof(Toggle), startDelay, interval);
        }

        private void Toggle()
        {
            this.isActive = !this.isActive;

            if (isActive)
            {
                lightSource.intensity = 0;
            }
            else
            {
                lightSource.intensity = defaultIntensity;
            }
        }
    }
}
