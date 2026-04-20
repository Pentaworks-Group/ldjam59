using System;

using Assets.Scripts.Core.Models;

using GameFrame.Core.Extensions;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

using UnityVector3 = UnityEngine.Vector3;

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
        private ConnectionLossEffect connectionLossEffect;
        [SerializeField]
        private PauseMenuBehaviour pauseMenuBehaviour;
        [SerializeField]
        private PauseSubMenuBehaviour hitSubMenuBehaviour;

        private GameObject signalObject;
        private Transform source;
        private TargetBehaviour target;

        private InputAction clickAction;
        private InputAction escapeAction;

        private int[] trackIndices = { 0, 3 }; //Track indices of soundTracks which track the Object

        private void OnDisable()
        {
            clickAction.performed -= OnLeftMouseClicked;
        }

        private void OnPauseToggled(Boolean isPaused)
        {
            if (!isPaused)
            {
                clickAction.performed += OnLeftMouseClicked;
            }
            else
            {
                clickAction.performed -= OnLeftMouseClicked;
            }
        }

        private void OnLeftMouseClicked(InputAction.CallbackContext context)
        {
            GameFrame.Base.Audio.Effects.Play("Launch");

            var activeSignal = new Signal();
            Base.Core.Game.State.CurrentLevel.ActiveSignals.Add(activeSignal);

            SpawnSignal(activeSignal, false);

            Base.Core.Game.State.CurrentLevel.Score.SignalsSend++;
            Base.Core.Game.State.CurrentLevel.SignalsSend++;
        }

        private void Awake()
        {
            clickAction = InputSystem.actions.FindAction("Click");
            escapeAction = InputSystem.actions.FindAction("Escape");

            Base.Core.Game.ExecuteAfterInstantation(Init);

            var audioEngine = Base.Audio.AudioEngine;

            audioEngine.SetCombination("Level_0", 1.0f);
            string[] combinations = { "Level_0", "Level_1", "Level_2", "Level_3" };
            audioEngine.StartRandomCycling(combinations, 1, 7, 0.2f);
            audioEngine.Play();

            OnTargetHit.AddListener(TargetHit);
        }



        private void Update()
        {
            if (Base.Core.Game.IsRunning)
            {
                Base.Core.Game.State.TimeElapsed += Time.deltaTime;
                Base.Core.Game.State.CurrentLevel.Score.LevelTime += Time.deltaTime;
                Base.Core.Game.State.CurrentLevel.TimeElapsed += Time.deltaTime;
            }
        }

        private void Init()
        {
            var currentLevel = Base.Core.Game.State.CurrentLevel;

            var tmpSource = SpawnObject(currentLevel.Source, objectTemplate).gameObject;
            tmpSource.SetActive(true);
            tmpSource.AddComponent<MouseTracker>();

            source = tmpSource.transform;

            var tmpTarget = SpawnObject(currentLevel.Target, targetTemplate).transform;
            tmpTarget.gameObject.SetActive(true);
            target = tmpTarget.GetComponent<TargetBehaviour>();

            signalObject = SpawnObject(currentLevel.Signal, signalTemplate).gameObject;

            if (currentLevel.ActiveSignals?.Count > 0)
            {
                foreach (var signal in currentLevel.ActiveSignals)
                {
                    SpawnSignal(signal, true);
                }
            }

            clickAction.performed += OnLeftMouseClicked;
            clickAction.Enable();

            escapeAction.Enable();

            Base.Core.Game.PauseToggled.AddListener(OnPauseToggled);
        }

        private SimpleObjectBehaviour SpawnObject(SimpleSpaceObject simpleSpaceObject, SimpleObjectBehaviour usedObjectTemplate)
        {
            var postion = new UnityEngine.Vector3(simpleSpaceObject.Position.Value.X, 0, simpleSpaceObject.Position.Value.Y);

            var objectBehaviour = Instantiate(usedObjectTemplate, postion, usedObjectTemplate.transform.rotation, transform);
            objectBehaviour.gameObject.name = simpleSpaceObject.Name;
            objectBehaviour.Init(simpleSpaceObject);

            return objectBehaviour;
        }

        private void SpawnSignal(Signal signal, Boolean isRestore)
        {
            var instance = GameObject.Instantiate(signalObject, signalContainer);

            if (isRestore)
            {
                instance.transform.position = signal.Position.ToUnity();
            }
            else
            {
                instance.transform.position = source.position;
            }

            instance.name = "pew";

            instance.SetActive(true);

            if (instance.TryGetComponent<Rigidbody>(out var rigidbody))
            {
                if (!isRestore)
                {
                    UnityVector3 mousePosition = Pointer.current.position.ReadValue();
                    mousePosition.z = Camera.main.transform.position.y;

                    var viewedMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                    viewedMousePosition.y = 0f;

                    var startPosition = new UnityVector3(source.position.x, 0f, source.position.z);

                    var vector = viewedMousePosition - startPosition;
                    rigidbody.rotation = Quaternion.LookRotation(vector);

                    rigidbody.AddForce(vector.normalized * Base.Core.Game.State.CurrentLevel.SignalVelocityFactor, ForceMode.Impulse);
                }
                else
                {
                    rigidbody.rotation = signal.Rotation.ToUnityQuaternion();
                    rigidbody.linearVelocity = signal.Force.ToUnity();
                }
            }

            if (instance.TryGetComponent<SignalBehaviour>(out var signalBehaviour))
            {
                signalBehaviour.Init(signal);

                if (Base.Core.Game.State.Mode.Audio.IsConnectionLossEnabled == true)
                {
                    signalBehaviour.InitConnectionLoss(source, connectionLossEffect);
                }
            }

            // Should be enabled, but as we have more than one object, not supported!
            /*
            if (Base.Core.Game.State.Mode.Audio?.IsTrackingObjects == true)
            {
                Base.Audio.AudioEngine.TrackObject(instance.transform, trackIndices);
            }
            */
        }

        private void TargetHit()
        {
            var currentLevel = Base.Core.Game.State.CurrentLevel;
            var score = currentLevel.Score;

            if (score.LeastSent > currentLevel.SignalsSend)
            {
                score.LeastSent = currentLevel.SignalsSend;
            }

            if (score.ShortestHitDuration > currentLevel.TimeElapsed)
            {
                score.ShortestHitDuration = currentLevel.TimeElapsed;
            }

            pauseMenuBehaviour.OpenMenu(hitSubMenuBehaviour);
        }
    }
}
