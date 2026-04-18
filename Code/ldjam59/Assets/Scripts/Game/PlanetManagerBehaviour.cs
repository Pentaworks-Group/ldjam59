using Assets.Scripts.Core.Models;
using System.Collections.Generic;
using UnityEngine;

using GameFrame.Core.Extensions;

namespace Assets.Scripts.Game
{
	public class PlanetManagerBehaviour : MonoBehaviour
	{
		[SerializeField]
		private GameObject PlanetTemplate;
		[SerializeField]
		private GameObject PlanetContainer;
		private List<GameObject> planets = new List<GameObject>();

        private void Awake()
        {
            Base.Core.Game.ExecuteAfterInstantation(SpawnPlanets);
        }

        private void SpawnPlanets()
		{
			foreach (var planet in Base.Core.Game.State.CurrentLevel.Planets) {
				planets.Add(SpawnPlanet(planet));
			}
		}

		private GameObject SpawnPlanet(Planet planet)
		{
			var postion = new UnityEngine.Vector3(planet.Position.Value.X, 0, planet.Position.Value.Y);
            Quaternion xQuat = Quaternion.AngleAxis(planet.Axis.Y, UnityEngine.Vector3.right);
            Quaternion yQuat = Quaternion.AngleAxis(planet.Axis.X, UnityEngine.Vector3.left);
            Quaternion rotation = yQuat * xQuat;

            var planetObject = Instantiate(PlanetTemplate, postion, rotation, PlanetContainer.transform);
			return planetObject;
		}
	}
}