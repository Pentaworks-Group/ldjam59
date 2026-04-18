using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assets.Scripts.Scenes.GameTest;

using UnityEngine;

namespace Assets.Scripts.Scenes
{
    public  class PlanetBehaviour : MonoBehaviour
    {
        [SerializeField] private Single gravity = 1;

        private readonly List<Rigidbody> affectedBodies = new List<Rigidbody>();

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.parent.TryGetComponent<BulletBehaviour>(out var bullet))
            {
                if (bullet.TryGetComponent<Rigidbody>(out var bulletRigitBody))
                {
                    bullet.OnImpact.AddListener(OnBulletImpact);
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
                    if (affectedBody != null)
                    {
                        var vector = transform.position - affectedBody.transform.position;

                        affectedBody.AddForce(vector.normalized * gravity);
                    }                    
                }
            }
        }

        private void OnBulletImpact(BulletBehaviour bullet)
        {
            if (bullet != default)
            {
                if (bullet.TryGetComponent<Rigidbody>(out var rigidbody))
                {
                    this.affectedBodies.Remove(rigidbody);
                }
            }
        }
    }
}
