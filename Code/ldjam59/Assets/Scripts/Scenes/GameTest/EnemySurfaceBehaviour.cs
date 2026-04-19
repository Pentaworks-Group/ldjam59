using UnityEngine;

namespace Assets.Scripts.Scenes.GameTest
{
    public class EnemySurfaceBehaviour : MonoBehaviour
    {
        public ScoreBehaviour scoreBehaviour;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.body.TryGetComponent<BulletBehaviour>(out var bullet))
            {
                Debug.Log("hit");
                scoreBehaviour.AddScore();
                bullet.OnImpact.Invoke(bullet);
                bullet.OnImpact.RemoveAllListeners();
                Destroy(collision.body.gameObject);
            }
        }
    }
}