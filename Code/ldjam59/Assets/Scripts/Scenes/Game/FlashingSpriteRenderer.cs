using System;

using UnityEngine;

namespace Assets.Scripts.Scenes.Game
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class FlashingSpriteRenderer : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Single interval = 0.5f;
        [SerializeField] private Single startDelay = 0f;
        [SerializeField] private Boolean isActive = false;

        private void Start()
        {
            if (isActive)
            {
                spriteRenderer.material.EnableKeyword("_EMISSION");
            }

            InvokeRepeating(nameof(Toggle), startDelay, interval);
        }

        private void Toggle()
        {
            this.isActive = !this.isActive;

            if (isActive)
            {
                spriteRenderer.material.EnableKeyword("_EMISSION");
            }
            else
            {
                spriteRenderer.material.DisableKeyword("_EMISSION");
            }
        }
    }
}
