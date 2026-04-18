using Assets.Scripts.Core.Models;
using GameFrame.Core.Extensions;
using GameFrame.Core.Media;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

namespace Assets.Scripts.Game
{
    public class PlanetBehaviour : MonoBehaviour
    {
        private Planet planet;


        public void Init(Planet planet)
        {
            this.planet = planet;
            transform.localScale = planet.Size.Value * UnityEngine.Vector3.one;

            UpdateShader();
        }


        private void UpdateShader()
        {
            Renderer rend = GetComponent<Renderer>();
            //MaterialPropertyBlock propBlock = new MaterialPropertyBlock();

            //rend.GetPropertyBlock(propBlock);
            PlanetLayer planetLayer = planet.Layers[0];
            var col = planetLayer.Color.Value;
            Material[] mats = rend.materials;
            Material mat  = mats[0];
            mat.SetColor("_WorldColor", col.ToUnity());
            mat.SetVector("_RotationSpeed", new UnityEngine.Vector2(planetLayer.RotationSpeed.Value, 0));
            var texture = GameFrame.Base.Resources.Manager.Textures.Get(planetLayer.Texture);
            mat.SetTexture("_WorldTexture", texture);
            //rend.SetPropertyBlock(propBlock);
            mats[1] = mat;
            rend.materials = mats;

            //var col = planet.Layers[0].Color.Value;
            //material.SetColor("_WorldColor", col.ToUnity());
            //material.SetColor("ddd", col.ToUnity());
            //Debug.Log("Test");
        }
    }
}
