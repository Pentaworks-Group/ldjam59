using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assets.Scripts.Scenes.GameTest;

using UnityEngine;

namespace Assets.Scripts.Scenes
{
    public  class PlanetBehabiour : MonoBehaviour
    {
        [SerializeField] private Single gravity = 1;

        private readonly List<Rigidbody> affectedBodies = new List<Rigidbody>();

        private void OnCollisionEnter(Collision collision)
        {
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.parent.TryGetComponent<BulletBehaviour>(out var bullet))
            {
                if (bullet.TryGetComponent<Rigidbody>(out var bulletRigitBody))
                {
                    affectedBodies.Add(bulletRigitBody);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.parent.TryGetComponent<BulletBehaviour>(out var bullet))
            {
                if (bullet.TryGetComponent<Rigidbody>(out var bulletRigitBody))
                {
                    affectedBodies.Remove(bulletRigitBody);
                }
            }
        }

        private void FixedUpdate()
        {
            if (this.affectedBodies.Count > 0)
            {
                foreach (var affectedBody in this.affectedBodies) 
                {
                    var vector = transform.position - affectedBody.transform.position;

                    affectedBody.AddForce(vector.normalized * gravity);
                }
            }
        }
    }
}
