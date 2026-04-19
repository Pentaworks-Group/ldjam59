using Assets.Scripts.Core.Models;
using Assets.Scripts.Scenes.GameTest;
using GameFrame.Core.Extensions;
using GameFrame.Core.Media;
using System;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

namespace Assets.Scripts.Scenes.Game
{
    public class PlanetBehaviour : MonoBehaviour
    {
        private Planet planet;
        [SerializeField]
        private PlanetInfluenceBehaviour planetInfluenceBehaviour;
        [SerializeField] private Single gravity = 1;
        private readonly List<Rigidbody> affectedBodies = new List<Rigidbody>();

        public void Init(Planet planet)
        {
            this.planet = planet;
            gravity = planet.Gravity.Value;
            transform.localScale = planet.Size.Value * UnityEngine.Vector3.one;
            gameObject.name = planet.Name;

            UpdateShader();
            UpdateSphereOfInfluence();
        }

        private void Update()
        {
            if (planet?.Gravity.Value != gravity)
            {
                planet.Gravity = gravity;
                UpdateSphereOfInfluence();
            }
        }

        private void UpdateSphereOfInfluence()
        {
            if (planetInfluenceBehaviour.TryGetComponent<SphereCollider>(out var sphereCollider))
            {
                var size = 3f;

                if (planet.Gravity.Value > 0)
                {
                    size = UnityEngine.Mathf.Sqrt(planet.Gravity.Value * 10f);
                }

                sphereCollider.radius = size;
            }
        }

        private void UpdateShader()
        {
            Renderer rend = GetComponent<Renderer>();
            MaterialPropertyBlock propBlock = new MaterialPropertyBlock();

            SetLayerValues(propBlock, planet.PlanetLayer, "_PlanetColor", "_PlanetRotationSpeed", "_PlanetTexture");
            SetLayerValues(propBlock, planet.SurfaceLayer, "_SurfaceColor", "_SurfaceRotationSpeed", "_SurfaceTexture");
            SetLayerValues(propBlock, planet.CloudLayer, "_CloudColor", "_CloudRotationSpeed", "_CloudTexture");
            rend.SetPropertyBlock(propBlock);
        }

        private static void SetLayerValues(MaterialPropertyBlock propBlock, PlanetLayer planetLayer, string colorName, string speedName, string textureName)
        {
            var col = planetLayer.Color.Value;

            UnityEngine.Color colli = col.ToUnity();
            propBlock.SetColor(colorName, colli);
            propBlock.SetVector(speedName, new UnityEngine.Vector2(planetLayer.RotationSpeed.Value, 0));
            var texture = GameFrame.Base.Resources.Manager.Textures.Get(planetLayer.Texture);
            propBlock.SetTexture(textureName, texture);
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

                        //Debug.Log($"Applied force: {force}.");

                        affectedBody.AddForce(vector.normalized * force);
                    }
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.body.TryGetComponent<SignalBehaviour>(out var signal))
            {
                signal.OnImpact.Invoke(signal);
                signal.OnImpact.RemoveAllListeners();

                Destroy(collision.body.gameObject);
            }
        }

        public void RegisterSignal(SignalBehaviour signal)
        {
            if (signal.TryGetComponent<Rigidbody>(out var signalRigidBody))
            {
                signal.OnImpact.AddListener(OnSignalImpact);
                affectedBodies.Add(signalRigidBody);
            }
        }

        public void DeRegisterSignal(SignalBehaviour signal)
        {
            if (signal.TryGetComponent<Rigidbody>(out var signalRigidBody))
            {
                affectedBodies.Remove(signalRigidBody);
            }
        }

        private void OnSignalImpact(SignalBehaviour signal)
        {
            if (signal != default)
            {
                if (signal.TryGetComponent<Rigidbody>(out var rigidbody))
                {
                    this.affectedBodies.Remove(rigidbody);
                }
            }
        }
    }
}
