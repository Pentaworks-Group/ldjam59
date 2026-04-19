using System;
using System.Collections.Generic;
using Assets.Scripts.Scenes.Game;

using UnityEngine;

namespace Assets.Scripts.Scenes.GameTest
{
    public  class PlanetBehaviour : MonoBehaviour
    {
        [SerializeField] private Single gravity = 1;
        private Single actualGravity = 0;

        private readonly List<Rigidbody> affectedBodies = new List<Rigidbody>();

        private void Update()
        {
            if (actualGravity != gravity)
            {
                actualGravity = gravity;
                UpdateSphereOfInfluence();
            }
        }

        private void UpdateSphereOfInfluence()
        {
            if (TryGetComponent<SphereCollider>(out var sphereCollider))
            {
                var size = 3f;

                if (actualGravity > 0)
                {
                    size = UnityEngine.Mathf.Sqrt(actualGravity * 10f);
                }

                sphereCollider.radius = size;
            }
        }

        private void Awake()
        {
            actualGravity = gravity;
            UpdateSphereOfInfluence();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.parent.TryGetComponent<SignalBehaviour>(out var bullet))
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
            if (other.transform.parent.TryGetComponent<SignalBehaviour>(out var bullet))
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

                        var force = gravity / vector.sqrMagnitude;

                        Debug.Log($"Applied force: {force}.");

                        affectedBody.AddForce(vector.normalized * force);
                    }                    
                }
            }
        }

        public void OnBulletImpact(SignalBehaviour bullet)
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
