using Assets.Scripts.Core.Models;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputSettings;


namespace Assets.Scripts.Scenes.Game
{
    public class PlanetManagerBehaviour : MonoBehaviour
    {
        [SerializeField]
        private PlanetBehaviour PlanetTemplate;
        [SerializeField]
        private GameObject PlanetContainer;
        private List<PlanetBehaviour> planets = new List<PlanetBehaviour>();

        private void Awake()
        {
            Base.Core.Game.ExecuteAfterInstantation(SpawnPlanets);
        }



        private void SpawnPlanets()
        {
            foreach (var planet in Base.Core.Game.State.CurrentLevel.Planets)
            {
                planets.Add(SpawnPlanet(planet));
            }
        }

        private PlanetBehaviour SpawnPlanet(Planet planet)
        {
            var postion = new UnityEngine.Vector3(planet.Position.Value.X, 0, planet.Position.Value.Y);
            Quaternion xQuat = Quaternion.AngleAxis(planet.Axis.Y, UnityEngine.Vector3.right);
            Quaternion yQuat = Quaternion.AngleAxis(planet.Axis.X, UnityEngine.Vector3.left);
            Quaternion rotation = yQuat * xQuat;

            var planetBehaviour = Instantiate(PlanetTemplate, postion, rotation, PlanetContainer.transform);
            planetBehaviour.Init(planet);
            planetBehaviour.gameObject.SetActive(true);
            return planetBehaviour;
        }
    }
}