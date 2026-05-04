using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine.InputSystem;

namespace Assets.Scripts.Extensions
{
    public static class InputActionExtensions
    {
        public static void Hook(this InputAction action, Action<InputAction.CallbackContext> onPerformed = default, Action<InputAction.CallbackContext> onStarted = default, Action<InputAction.CallbackContext> onCancelled = default)
        {
            if (action != default)
            {
                if (onPerformed != default)
                {
                    action.performed += onPerformed;
                }

                if (onStarted != default)
                {
                    action.started += onStarted;
                }

                if (onCancelled != default)
                {
                    action.canceled += onCancelled;
                }
            }
        }
    }
}
