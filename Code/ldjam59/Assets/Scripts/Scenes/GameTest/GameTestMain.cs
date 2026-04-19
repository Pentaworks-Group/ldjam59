using Assets.Scripts.Scenes.Game;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Scenes.GameTest
{
    public class GameTestMain : MonoBehaviour
    {
        private InputAction leftMouseClick;

        [SerializeField]
        private GameObject bullet;
        
        [SerializeField]
        private Transform bulletContainer;
        
        [SerializeField]
        private Transform start;

        private void OnLeftMouseClicked()
        {
            Base.Core.Game.PlayButtonSound();

            var instance = GameObject.Instantiate(bullet, bulletContainer);
            instance.transform.position = start.position;

            instance.name = "pew";

            instance.SetActive(true);

            if (instance.TryGetComponent<Rigidbody>(out var rigidbody))
            {
                instance.AddComponent<BulletBehaviour>();

                Vector3 mousePosition = Mouse.current.position.ReadValue();
                mousePosition.z = Camera.main.transform.position.y;

                var viewedMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                viewedMousePosition.y = 0f;

                var startPosition = new Vector3(start.position.x, 0f, start.position.z);

                var vector = viewedMousePosition - startPosition;

                rigidbody.AddForce(vector.normalized * 2, ForceMode.Impulse);
            }
        }

        private void Awake()
        {
            leftMouseClick = new InputAction(binding: "<Mouse>/leftButton");
            leftMouseClick.performed += ctx => OnLeftMouseClicked();
            leftMouseClick.Enable();
        }
    }
}
