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

        private void Awake()
        {
            OnImpact.AddListener(DestroySignal);
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
