using UnityEngine;
namespace Assets.Scripts.Scenes.Game
{
    public class TargetBehaviour : MonoBehaviour
    {
        [SerializeField]
        private GameMainBehaviour main;

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.parent.TryGetComponent<SignalBehaviour>(out var signal))
            {
                signal.OnImpact.Invoke(signal);
                signal.OnImpact.RemoveAllListeners();
                main.OnTargetHit.Invoke();

                Destroy(other.transform.parent.gameObject);
            }
        }
    }
}