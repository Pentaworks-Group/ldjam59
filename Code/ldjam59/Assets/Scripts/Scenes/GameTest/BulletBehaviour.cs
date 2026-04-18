using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Scenes.GameTest
{
    public class BulletBehaviour : MonoBehaviour
    {
        public UnityEvent<BulletBehaviour> OnImpact = new UnityEvent<BulletBehaviour>();

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
            if (other is BoxCollider)
            {
                Destroy(gameObject);
            }
        }
    }
}
