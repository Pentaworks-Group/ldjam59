using Assets.Scripts.Core.Models;
using GameFrame.Core.Extensions;
using UnityEngine;

namespace Assets.Scripts.Scenes.Game
{
	public class SimpleObjectBehaviour : MonoBehaviour
	{
		[SerializeField]
		private SpriteRenderer spriteRenderer;
		private SimpleSpaceObject simpleSpaceObject;


        public void Init(SimpleSpaceObject spaceObject)
		{
			simpleSpaceObject = spaceObject;
			UpdateRendering();
            Base.Core.Game.OnModelUpdate.AddListener(UpdateModel);
        }


		private void UpdateRendering()
		{
            spriteRenderer.sprite = GameFrame.Base.Resources.Manager.Sprites.Get(simpleSpaceObject.Sprite);
            spriteRenderer.color = simpleSpaceObject.Color.ToUnity();
			transform.localScale = new UnityEngine.Vector3(simpleSpaceObject.Size.Value, simpleSpaceObject.Size.Value, simpleSpaceObject.Size.Value);
        }

		private void UpdateModel()
		{
			simpleSpaceObject.Color = spriteRenderer.color.ToFrame();
			var pos = transform.position;
			simpleSpaceObject.Position = new GameFrame.Core.Math.Vector2(pos.x, pos.z);
			simpleSpaceObject.Size = transform.localScale.x;
			simpleSpaceObject.Sprite = spriteRenderer.sprite.name;
        }
	}
}