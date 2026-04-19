using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Scenes.Game
{
    public class MouseTracker : MonoBehaviour
    {
        private void Update()
        {
            if (Base.Core.Game.IsRunning)
            {
                Vector3 mousePosition = Pointer.current.position.ReadValue();
                mousePosition.z = Camera.main.transform.position.y;
                var viewedMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

                viewedMousePosition.y = transform.position.y;

                var targetRotation = Quaternion.LookRotation(transform.position - viewedMousePosition);
                transform.rotation = targetRotation;
            }
        }
    }
}
