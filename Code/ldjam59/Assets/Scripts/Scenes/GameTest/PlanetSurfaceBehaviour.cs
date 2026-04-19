using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts.Scenes.GameTest
{
    public class PlanetSurfaceBehaviour : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.body.TryGetComponent<BulletBehaviour>(out var bullet))
            {
                bullet.OnImpact.Invoke(bullet);
                bullet.OnImpact.RemoveAllListeners();

                Destroy(collision.body.gameObject);
            }
        }
    }
}
