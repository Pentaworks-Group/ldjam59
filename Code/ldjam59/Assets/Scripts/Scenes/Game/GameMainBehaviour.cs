using Assets.Scripts.Core.Models;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Scenes.Game
{
    public class GameMainBehaviour : MonoBehaviour
    {
        public UnityEvent OnTargetHit { get; set; } = new UnityEvent();


        [SerializeField]
        private SimpleObjectBehaviour objectTemplate;
        [SerializeField]
        private SimpleObjectBehaviour signalTemplate;
        [SerializeField]
        private SimpleObjectBehaviour targetTemplate;
        [SerializeField]
        private Transform signalContainer;

        private GameObject signal;
        private Transform source;
        private TargetBehaviour target;

        private InputAction leftMouseClick;

        private void OnLeftMouseClicked()
        {
            Base.Core.Game.PlayButtonSound();

            var instance = GameObject.Instantiate(signal, signalContainer);
            instance.transform.position = source.position;

            instance.name = "pew";

            instance.SetActive(true);

            if (instance.TryGetComponent<Rigidbody>(out var rigidbody))
            {
                Vector3 mousePosition = Mouse.current.position.ReadValue();
                mousePosition.z = Camera.main.transform.position.y;

                var viewedMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                viewedMousePosition.y = 0f;

                var startPosition = new Vector3(source.position.x, 0f, source.position.z);

                var vector = viewedMousePosition - startPosition;
                rigidbody.rotation = Quaternion.LookRotation(vector);

                rigidbody.AddForce(vector.normalized * 2, ForceMode.Impulse);
            }
        }

        private void Awake()
        {
            leftMouseClick = new InputAction(binding: "<Mouse>/leftButton");
            leftMouseClick.performed += ctx => OnLeftMouseClicked();

            Base.Core.Game.ExecuteAfterInstantation(Init);
        }

        private void Init()
        {
            Debug.Log("Main Init");

            var tmpSource = SpawnObject(Base.Core.Game.State.CurrentLevel.Source, objectTemplate).gameObject;
            tmpSource.SetActive(true);
            tmpSource.AddComponent<MouseTracker>();
            source = tmpSource.transform;
            var tmpTarget = SpawnObject(Base.Core.Game.State.CurrentLevel.Target, targetTemplate).transform;
            tmpTarget.gameObject.SetActive(true);
            target = tmpTarget.GetComponent<TargetBehaviour>();


            signal = SpawnObject(Base.Core.Game.State.CurrentLevel.Signal, signalTemplate).gameObject;


            leftMouseClick.Enable();
        }


        private SimpleObjectBehaviour SpawnObject(SimpleSpaceObject simpleSpaceObject, SimpleObjectBehaviour usedObjectTemplate)
        {
            var postion = new UnityEngine.Vector3(simpleSpaceObject.Position.Value.X, 0, simpleSpaceObject.Position.Value.Y);

            var objectBehaviour = Instantiate(usedObjectTemplate, postion, usedObjectTemplate.transform.rotation, transform);
            objectBehaviour.gameObject.name = simpleSpaceObject.Name;
            objectBehaviour.Init(simpleSpaceObject);
            return objectBehaviour;
        }
    }
}
