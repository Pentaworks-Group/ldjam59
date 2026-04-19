using System;

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
        [SerializeField]
        private AudioEngine audioEngine;

        private GameObject signal;
        private Transform source;
        private TargetBehaviour target;

        private InputAction clickAction;
        private InputAction escapeAction;

        private int[] trackIndices = { 0, 3 }; //Track indices of soundTracks which track the Object

        private void OnDisable()
        {
            clickAction.performed -= OnLeftMouseClicked;
            escapeAction.performed -= OnEscapePressed;
        }

        private void OnEscapePressed(InputAction.CallbackContext context)
        {
            if (Base.Core.Game.IsRunning)
            {
                Base.Core.Game.Pause();

                
            }
        }

        private void OnPauseToggled(Boolean isPaused)
        {
            if (!isPaused)
            {
                clickAction.performed += OnLeftMouseClicked;
                escapeAction.performed += OnEscapePressed;
            }
            else
            {
                clickAction.performed -= OnLeftMouseClicked;
                escapeAction.performed -= OnEscapePressed;
            }
        }

        private void OnLeftMouseClicked(InputAction.CallbackContext context)
        {
            Base.Core.Game.PlayButtonSound();

            var instance = GameObject.Instantiate(signal, signalContainer);
            instance.transform.position = source.position;

            instance.name = "pew";

            instance.SetActive(true);

            if (instance.TryGetComponent<Rigidbody>(out var rigidbody))
            {
                Vector3 mousePosition = Pointer.current.position.ReadValue();
                mousePosition.z = Camera.main.transform.position.y;

                var viewedMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                viewedMousePosition.y = 0f;

                var startPosition = new Vector3(source.position.x, 0f, source.position.z);

                var vector = viewedMousePosition - startPosition;
                rigidbody.rotation = Quaternion.LookRotation(vector);

                rigidbody.AddForce(vector.normalized * 2, ForceMode.Impulse);
            }

            if (instance.TryGetComponent<SignalBehaviour>(out var signalBehaviour))
            {
                signalBehaviour.SetBase(source);
                signalBehaviour.SetAudioEngine(audioEngine);
            }
        }

        private void Awake()
        {
            clickAction = InputSystem.actions.FindAction("Click");
            escapeAction = InputSystem.actions.FindAction("Escape");

            Base.Core.Game.ExecuteAfterInstantation(Init);

            audioEngine.SetCombination("Level_0", 1.0f);
            string[] combinations = { "Level_0", "Level_1", "Level_2", "Level_3" };
            audioEngine.StartRandomCycling(combinations, 1, 7, 0.2f);
            audioEngine.Play();
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

            clickAction.performed += OnLeftMouseClicked;
            clickAction.Enable();

            escapeAction.performed += OnEscapePressed;
            escapeAction.Enable();

            Base.Core.Game.PauseToggled.AddListener(OnPauseToggled);
        }

        private SimpleObjectBehaviour SpawnObject(SimpleSpaceObject simpleSpaceObject, SimpleObjectBehaviour usedObjectTemplate)
        {
            var postion = new UnityEngine.Vector3(simpleSpaceObject.Position.Value.X, 0, simpleSpaceObject.Position.Value.Y);

            var objectBehaviour = Instantiate(usedObjectTemplate, postion, usedObjectTemplate.transform.rotation, transform);
            objectBehaviour.gameObject.name = simpleSpaceObject.Name;
            objectBehaviour.Init(simpleSpaceObject);

            audioEngine.TrackObject(objectBehaviour.transform, trackIndices);
            //TODO: untrack if the object gets removed

            return objectBehaviour;
        }
    }
}
