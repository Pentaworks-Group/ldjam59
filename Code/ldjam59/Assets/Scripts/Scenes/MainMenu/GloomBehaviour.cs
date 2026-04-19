using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Assets.Scripts.Scenes.MainMenu
{
    [RequireComponent(typeof(Image))]
    public class GloomBehaviour : MonoBehaviour
    {
        private Boolean isPaused;
        private GameObject parent;
        private Image MaskImage;
        [SerializeField]
        private float speed = 0.05f;
        [SerializeField]
        private float intensity = 0.7f;

        private float currentAlpha = 0;

        private void Awake()
        {
            if (TryGetComponent<Image>(out MaskImage))
            {
                this.parent = this.transform.parent.gameObject;

                EventTrigger trigger = parent.AddComponent<EventTrigger>();

                AddTrigger(trigger, EventTriggerType.PointerEnter, Pause);
                AddTrigger(trigger, EventTriggerType.PointerExit, Resume);

                StartCoroutine(Gloom());
            }
        }

        public void Pause(BaseEventData baseEventData)
        {
            this.isPaused = true;
            currentAlpha = 0;
            this.MaskImage.color = new Color(this.MaskImage.color.r, this.MaskImage.color.g, this.MaskImage.color.b, currentAlpha);
        }

        public void Resume(BaseEventData baseEventData)
        {
            this.isPaused = false;
        }

        private void AddTrigger(EventTrigger trigger, EventTriggerType type, UnityAction<BaseEventData> action)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry()
            {
                eventID = type
            };

            entry.callback.AddListener(action);
            trigger.triggers.Add(entry);
        }

        private IEnumerator Gloom()
        {
            currentAlpha = this.MaskImage.color.a;

            while (true)
            {
                while (currentAlpha < intensity)
                {
                    currentAlpha = Mathf.Lerp(0, 1, currentAlpha);

                    while (isPaused) yield return null;

                    this.MaskImage.color = new Color(this.MaskImage.color.r, this.MaskImage.color.g, this.MaskImage.color.b, currentAlpha);
                    yield return new WaitForSeconds(.05f);
                    currentAlpha += speed;
                }

                currentAlpha = intensity;

                while (isPaused) yield return null;

                this.MaskImage.color = new Color(this.MaskImage.color.r, this.MaskImage.color.g, this.MaskImage.color.b, currentAlpha);

                yield return new WaitForSeconds(.1f);

                while (currentAlpha > 0)
                {
                    currentAlpha = Mathf.Lerp(0, 1, currentAlpha);

                    while (isPaused) yield return null;

                    this.MaskImage.color = new Color(this.MaskImage.color.r, this.MaskImage.color.g, this.MaskImage.color.b, currentAlpha);
                    yield return new WaitForSeconds(.05f);
                    currentAlpha -= speed;
                }

                currentAlpha = 0;

                while (isPaused) yield return null;

                this.MaskImage.color = new Color(this.MaskImage.color.r, this.MaskImage.color.g, this.MaskImage.color.b, currentAlpha);
                yield return new WaitForSeconds(.5f);
            }
        }
    }
}
