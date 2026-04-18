using Assets.Scripts.Core.Definitions;
using Assets.Scripts.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

namespace Assets.Scripts.Scenes.GameTest
{
    public class MouseTracker : MonoBehaviour
    {

        private void Update()
        {
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            mousePosition.z = 10f;
            var viewedMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            //Debug.Log("viewedMousePosition:" + viewedMousePosition);
            //Debug.Log("mousePosition:" + mousePosition);
            //var angle = Vector3.Angle(viewedMousePosition, transform.position);
            //Debug.Log(transform.rotation.ToString());
            //Debug.Log(angle.ToString());

            //var mousePosition = Mouse.current.position.ReadValue();
            var targetRotation = Quaternion.LookRotation(viewedMousePosition - transform.position);
            transform.rotation = targetRotation;
            //transform.Rotate(new Vector3(mousePosition.x, 0, mousePosition.y), Space.World);

        }
    }
}
