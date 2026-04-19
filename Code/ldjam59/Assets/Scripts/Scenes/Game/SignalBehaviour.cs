using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Scenes.Game
{
    public class SignalBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Collider borderCollider;

        public UnityEvent<SignalBehaviour> OnImpact = new UnityEvent<SignalBehaviour>();

        private AudioEngine audioEngine;

        private Transform baseObject;

        private void Awake()
        {
            OnImpact.AddListener(DestroySignal);
        }

        
        public void SetBase(Transform baseObject)
        {
            this.baseObject = baseObject;
        }

        public void SetAudioEngine(AudioEngine audioEngine)
        {
            this.audioEngine = audioEngine;
        }

        private void Update()
        {
            //TODO: this approach fails if there are multiple objects
            if(baseObject != null)
            {
                // Calculate direction and distance to the target
                float distance = Vector3.Distance(transform.position, baseObject.position);

                bool shouldBeActive = false;

                if (Physics.Raycast(transform.position, baseObject.position, out var hit, distance))
                {
                    if(hit.transform != baseObject) {
                        shouldBeActive = true;
                    }
                }

                if (audioEngine.IsRadioEffectActive != shouldBeActive)
                {
                    if(shouldBeActive)
                    {
                        audioEngine.EnableRadioEffect(true, 0.2f);
                        //Debug.Log("[SignalBehaviour] EnableRadioEffect");
                    }
                    else
                    {
                        audioEngine.DisableRadioEffect(true, 0.2f);
                        //Debug.Log("[SignalBehaviour] DisableRadioEffect");
                    }
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            
        }

        private void OnCollisionExit(Collision collision)
        {
            
        }

        private void OnTriggerEnter(Collider other)
        {
            
        }

        private void OnTriggerExit(Collider other)
        {
            if (other == borderCollider)
            {
                OnImpact.Invoke(this);
            }
        }

        private void DestroySignal(SignalBehaviour behaviour)
        {
            OnImpact.RemoveAllListeners();
            Destroy(gameObject);
        }
    }
}
