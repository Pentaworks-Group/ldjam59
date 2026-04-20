
using Assets.Scripts.Core.Models;

using GameFrame.Core.Extensions;

using UnityEngine;
using UnityEngine.Events;

using UVector3 = UnityEngine.Vector3;

namespace Assets.Scripts.Scenes.Game
{
    public class SignalBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Collider borderCollider;

        public UnityEvent<SignalBehaviour> OnImpact = new UnityEvent<SignalBehaviour>();

        private ConnectionLossEffect connectionLossEffect;

        private Transform baseObject;
        private Rigidbody activeRigidbody;

        public Signal Signal { get; private set; }

        private void Awake()
        {
            OnImpact.AddListener(DestroySignal);
            
            if (TryGetComponent<Rigidbody>(out var rigidbody))
            {
                activeRigidbody = rigidbody;
            }
        }

        public void Init(Signal signal, Transform baseObject)
        {
            this.Signal = signal;
            this.baseObject = baseObject;
        }

        public void SetConnectionLossEffect(ConnectionLossEffect connectionLossEffect)
        {
            this.connectionLossEffect = connectionLossEffect;
        }

        private void Update()
        {
            if (Base.Core.Game.IsRunning)
            {
                if (this.Signal != default)
                {
                    this.Signal.Position = transform.position.ToFrame();
                    this.Signal.Force = this.activeRigidbody.GetAccumulatedForce().ToFrame();
                }
            }

            //TODO: this approach fails if there are multiple objects
            if (baseObject != null)
            {
                // Calculate direction and distance to the target
                float distance = UVector3.Distance(transform.position, baseObject.position);

                bool shouldBeActive = false;

                if (Physics.Raycast(transform.position, baseObject.position, out var hit, distance))
                {
                    if (hit.transform != baseObject)
                    {
                        shouldBeActive = true;
                    }
                }

                var audioEngine = Base.Audio.AudioEngine;

                if (audioEngine.IsRadioEffectActive != shouldBeActive)
                {
                    if (shouldBeActive)
                    {
                        audioEngine.EnableRadioEffect(true, 0.2f);
                    }
                    else
                    {
                        audioEngine.DisableRadioEffect(true, 0.2f);
                    }
                }

                if (connectionLossEffect != null && shouldBeActive != connectionLossEffect.IsEffectActive)
                {
                    connectionLossEffect.SetConnected(shouldBeActive);
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
            Base.Core.Game.State.CurrentLevel.ActiveSignals.Remove(behaviour.Signal);

            Destroy(gameObject);
        }
    }
}
