using Assets.Scripts.Scenes.Game;

using UnityEngine;

namespace Assets.Scripts.Scenes.GameTest
{
    public class EnemySurfaceBehaviour : MonoBehaviour
    {
        public ScoreBehaviour scoreBehaviour;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.body.TryGetComponent<SignalBehaviour>(out var signalBehaviour))
            {
                Debug.Log("hit");
                scoreBehaviour.AddScore();

                signalBehaviour.OnImpact.Invoke(signalBehaviour);
                signalBehaviour.OnImpact.RemoveAllListeners();

                Destroy(collision.body.gameObject);
            }
        }
    }
}