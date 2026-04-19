using Assets.Scripts.Core.Models;
using GameFrame.Core.Extensions;
using GameFrame.Core.Media;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

namespace Assets.Scripts.Scenes.Game
{
    public class PlanetBehaviour : MonoBehaviour
    {
        private Planet planet;

        public void Init(Planet planet)
        {
            this.planet = planet;
            transform.localScale = planet.Size.Value * UnityEngine.Vector3.one;
            gameObject.name = planet.Name;

            UpdateShader();
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
    }
}
