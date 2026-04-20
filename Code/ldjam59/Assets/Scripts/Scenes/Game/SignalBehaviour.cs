
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
        private ConnectionLossEffect connectionLossEffect;

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

        public void SetConnectionLossEffect(ConnectionLossEffect connectionLossEffect)
        {
            this.connectionLossEffect = connectionLossEffect;
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
                    }
                    else
                    {
                        audioEngine.DisableRadioEffect(true, 0.2f);
                    }
                }

                if (connectionLossEffect != null && shouldBeActive!=connectionLossEffect.IsEffectActive)
                {
                    if(shouldBeActive)
                    {
                        //Debug.Log("Set Connection Loss effect active");
                        connectionLossEffect.SetConnected(false);
                    }
                    else
                    {
                        //Debug.Log("Set Connection Loss effect inactive");
                        connectionLossEffect.SetConnected(true);
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
