using UnityEngine;

namespace Assets.Scripts.Scenes.Game
{
    public class PlanetInfluenceBehaviour : MonoBehaviour
    {
        [SerializeField]
        private PlanetBehaviour planetBehaviour;

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.parent.TryGetComponent<SignalBehaviour>(out var signal))
            {
                planetBehaviour.RegisterSignal(signal);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.parent.TryGetComponent<SignalBehaviour>(out var signal))
            {
                planetBehaviour.DeRegisterSignal(signal);
            }
        }
    }
}
