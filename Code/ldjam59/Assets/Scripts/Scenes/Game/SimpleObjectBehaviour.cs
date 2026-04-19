using Assets.Scripts.Core.Models;
using GameFrame.Core.Extensions;
using UnityEngine;

namespace Assets.Scripts.Scenes.Game
{
	public class SimpleObjectBehaviour : MonoBehaviour
	{
		[SerializeField]
		private SpriteRenderer spriteRenderer;
		private SimpleSpaceObject SimpleSpaceObject;
		public void Init(SimpleSpaceObject spaceObject)
		{
			SimpleSpaceObject = spaceObject;
			UpdateRendering();
		}


		private void UpdateRendering()
		{
            spriteRenderer.sprite = GameFrame.Base.Resources.Manager.Sprites.Get(SimpleSpaceObject.Sprite);
            spriteRenderer.color = SimpleSpaceObject.Color.ToUnity();
			transform.localScale = new UnityEngine.Vector3(SimpleSpaceObject.Size.Value, SimpleSpaceObject.Size.Value, SimpleSpaceObject.Size.Value);
        }
	}
}