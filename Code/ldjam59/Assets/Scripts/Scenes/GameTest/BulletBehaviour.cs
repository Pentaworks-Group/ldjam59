using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts.Scenes.GameTest
{
    public class BulletBehaviour : MonoBehaviour
    {
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
