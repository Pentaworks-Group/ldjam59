using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assets.Scripts.Core.Definitions;
using Assets.Scripts.Core.Models;

using Unity.Mathematics;

using UnityEditor.Rendering;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Scenes.GameTest
{
    public class MouseTracker : MonoBehaviour
    {

        private void Update()
        {
            //var mousePosition = Mouse.current.position.ReadValue();

            //var viewedMousePosition = Camera.main.ViewportToWorldPoint(mousePosition);

            //Debug.Log(transform.rotation.ToString());
            //var angle = Vector3.Angle(mousePosition, transform.position);
            //Debug.Log(transform.rotation.ToString());
            //Debug.Log(angle.ToString());

            var mousePosition = Mouse.current.position.ReadValue();

            transform.Rotate(new Vector3(mousePosition.x, 0, mousePosition.y), Space.World);

        }
    }
}
