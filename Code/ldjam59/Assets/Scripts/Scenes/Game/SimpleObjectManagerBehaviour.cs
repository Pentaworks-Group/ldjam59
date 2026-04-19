using Assets.Scripts.Core.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Scenes.Game
{
    public class SimpleObjectManagerBehaviour : MonoBehaviour
    {
        [SerializeField]
        private SimpleObjectBehaviour objectTemplate;
        private List<SimpleObjectBehaviour> objects = new List<SimpleObjectBehaviour>();

        private void Awake()
        {
            Base.Core.Game.ExecuteAfterInstantation(SpawnObjects);
        }

        private void SpawnObjects()
        {
            SpawnObject(Base.Core.Game.State.CurrentLevel.Source);
            SpawnObject(Base.Core.Game.State.CurrentLevel.Target);
            SpawnObject(Base.Core.Game.State.CurrentLevel.Signal);
        }

        private SimpleObjectBehaviour SpawnObject(SimpleSpaceObject simpleSpaceObject)
        {
            var postion = new UnityEngine.Vector3(simpleSpaceObject.Position.Value.X, 0, simpleSpaceObject.Position.Value.Y);

            var objectBehaviour = Instantiate(objectTemplate, postion, objectTemplate.transform.rotation, transform);
            objectBehaviour.Init(simpleSpaceObject);
            objectBehaviour.gameObject.SetActive(true);
            objects.Add(objectBehaviour);
            return objectBehaviour;
        }
    }
}
