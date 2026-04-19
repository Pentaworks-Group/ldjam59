using Assets.Scripts.Scenes.Game;


using UnityEngine;

namespace Assets.Scripts.Scenes.GameTest
{
    public class PlanetInfluenceBehaviour : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.body.TryGetComponent<BulletBehaviour>(out var bullet))
            {
                bullet.OnImpact.Invoke(bullet);
                bullet.OnImpact.RemoveAllListeners();

                Destroy(collision.body.gameObject);
            }
        }
    }
}
