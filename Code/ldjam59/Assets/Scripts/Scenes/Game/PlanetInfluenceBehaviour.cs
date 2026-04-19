using UnityEngine;

namespace Assets.Scripts.Scenes.Game
{
    public class PlanetInfluenceBehaviour : MonoBehaviour
    {
        [SerializeField]
        private PlanetBehaviour planetBehaviour;

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.parent.TryGetComponent<BulletBehaviour>(out var bullet))
            {
                planetBehaviour.RegisterBullet(bullet);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.parent.TryGetComponent<BulletBehaviour>(out var bullet))
            {
                planetBehaviour.DeRegisterBullet(bullet);
            }
        }
    }
}
