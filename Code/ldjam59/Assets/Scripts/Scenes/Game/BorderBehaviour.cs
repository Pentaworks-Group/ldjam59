using UnityEngine;
namespace Assets.Scripts.Scenes.Game
{
	public class BorderBehaviour : MonoBehaviour
	{
        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log($"Collision: {collision.body}");
        }
    }
}
