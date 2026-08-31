using System;

using UnityEngine;

namespace Assets.Scripts.Prefabs.Menu.Pause
{
    public class PauseSubMenuBehaviour : MonoBehaviour
    {
        public String title;

        public virtual Boolean CanClose()
        {
            return true;
        }
    }
}
