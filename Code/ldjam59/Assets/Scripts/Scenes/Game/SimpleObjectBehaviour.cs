using Assets.Scripts.Core.Models;
using GameFrame.Core.Extensions;
using UnityEngine;

namespace Assets.Scripts.Scenes.Game
{
	public class SimpleObjectBehaviour : MonoBehaviour
	{
		private SimpleSpaceObject SimpleSpaceObject;
		public void Init(SimpleSpaceObject spaceObject)
		{
			SimpleSpaceObject = spaceObject;
			UpdateRendering();
		}


		private void UpdateRendering()
		{
            SpriteRenderer rend = GetComponent<SpriteRenderer>();
			rend.sprite = GameFrame.Base.Resources.Manager.Sprites.Get(SimpleSpaceObject.Sprite);
			rend.color = SimpleSpaceObject.Color.ToUnity();
			transform.localScale = new UnityEngine.Vector3(SimpleSpaceObject.Size.Value, 1f, SimpleSpaceObject.Size.Value);
        }
	}
}