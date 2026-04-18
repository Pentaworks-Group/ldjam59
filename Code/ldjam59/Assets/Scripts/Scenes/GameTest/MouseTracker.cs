using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Scenes.GameTest
{
    public class MouseTracker : MonoBehaviour
    {
        private void Update()
        {
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            mousePosition.z = Camera.main.transform.position.y;
            var viewedMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            viewedMousePosition.y = transform.position.y;

            var targetRotation = Quaternion.LookRotation(viewedMousePosition - transform.position);
            transform.rotation = targetRotation;
        }
    }
}
