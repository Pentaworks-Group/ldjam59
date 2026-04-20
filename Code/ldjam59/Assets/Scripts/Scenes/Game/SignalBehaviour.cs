
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Scenes.Game
{
    public class SignalBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Collider borderCollider;

        public UnityEvent<SignalBehaviour> OnImpact = new UnityEvent<SignalBehaviour>();

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

                var audioEngine = Base.Audio.AudioEngine;

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
                        connectionLossEffect.SetConnected(false);
                    else
                        connectionLossEffect.SetConnected(true);
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
