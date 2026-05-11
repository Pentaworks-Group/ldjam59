using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.InputSystem;
using Assets.Scripts.Extensions;

using UnityVector3 = UnityEngine.Vector3;
using System.Collections;

namespace Assets.Scripts.Scenes.Game
{
    public  class SatelliteBehaviour : MonoBehaviour
    {
        private IEnumerator turnLeft;
        private IEnumerator turnRight;

        private InputAction turnLeftAction;
        private InputAction turnRightAction;
        private InputAction accelerateAction;

        private void Start()
        {
            turnLeft = Turn(UnityVector3.down);
            turnRight = Turn(UnityVector3.up);

            turnLeftAction = InputSystem.actions.FindAction("Movement-TurnLeft");
            turnRightAction = InputSystem.actions.FindAction("Movement-TurnRight");
            accelerateAction = InputSystem.actions.FindAction("Movement-Accelerate");

            turnLeftAction.Hook(onStarted: OnTurnLeftStarted, onCancelled: OnTurnLeftCancelled);
            turnRightAction.Hook(onStarted: OnTurnRightStarted, onCancelled: OnTurnRightCancelled);
            accelerateAction.Hook(onStarted: OnAccelerateStarted, onCancelled: OnAccelerateCancelled);
        }

        private void OnTurnLeftStarted(InputAction.CallbackContext _)
        {
            StartCoroutine(turnLeft);
        }

        private void OnTurnLeftCancelled(InputAction.CallbackContext _)
        {
            StopCoroutine(turnLeft);
        }

        private void OnTurnRightStarted(InputAction.CallbackContext _)
        {
            StartCoroutine(turnRight);
        }

        private void OnTurnRightCancelled(InputAction.CallbackContext _)
        {
            StopCoroutine(turnRight);
        }

        private void OnAccelerateStarted(InputAction.CallbackContext context)
        {
            StartCoroutine(nameof(Accelerate));
        }

        private void OnAccelerateCancelled(InputAction.CallbackContext context)
        {
            StopCoroutine(nameof(Accelerate));
        }

        private IEnumerator Turn(UnityVector3 turnVector)
        {
            var actialRotation = 100 * Time.deltaTime * turnVector;

            while (true)
            {
                this.transform.Rotate(actialRotation);
                yield return new WaitForFixedUpdate();
            }
        }

        private IEnumerator Accelerate()
        {
            if (TryGetComponent<Rigidbody>(out var sourceRigidbody))
            {
                while (true)
                {
                    sourceRigidbody.AddForce(transform.forward * Time.deltaTime, ForceMode.Impulse);
                    yield return new WaitForFixedUpdate();
                }
            }

            yield break;
        }
    }
}
