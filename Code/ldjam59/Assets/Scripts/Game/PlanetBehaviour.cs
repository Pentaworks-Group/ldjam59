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
            gameObject.name = planet.Name;

            UpdateShader();
        }


        private void UpdateShader()
        {
            Debug.Log($"planetName: {planet.Name}");
            Renderer rend = GetComponent<Renderer>();
            //MaterialPropertyBlock propBlock = new MaterialPropertyBlock();

            var propBlock = gameObject.GetComponent<MeshRenderer>().material;

            //rend.GetPropertyBlock(propBlock);
            SetLayerValues(propBlock, planet.PlanetLayer, "_PlanetColor", "_PlanetRotationSpeed", "_PlanetTexture");
            //rend.SetPropertyBlock(propBlock);
            SetLayerValues(propBlock, planet.SurfaceLayer, "_SurfaceColor", "_SurfaceRotationSpeed", "_SurfaceTexture");
            //rend.SetPropertyBlock(propBlock);
            SetLayerValues(propBlock, planet.CloudLayer, "_CloudColor", "_CloudRotationSpeed", "_CloudTexture");
            //rend.SetPropertyBlock(propBlock);
            //rend.material = propBlock;  
        }

        private static void SetLayerValues(Material propBlock, PlanetLayer planetLayer, string colorName, string speedName, string textureName)
        {
            var col = planetLayer.Color.Value;
            Debug.Log($"{colorName}: {planetLayer.Color.Value}");
            Debug.Log(speedName + ": " + planetLayer.RotationSpeed.Value);
            Debug.Log(textureName + ": " + planetLayer.Texture);

            UnityEngine.Color colli = col.ToUnity();
            Debug.Log($"colli: {colli}");
            propBlock.SetColor(colorName, colli);
            propBlock.SetVector(speedName, new UnityEngine.Vector2(planetLayer.RotationSpeed.Value, 0));
            var texture = GameFrame.Base.Resources.Manager.Textures.Get(planetLayer.Texture);
            propBlock.SetTexture(textureName, texture);
        }
    }
}
