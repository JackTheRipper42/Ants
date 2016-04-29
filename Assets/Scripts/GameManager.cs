﻿using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public GameObject AntPrefab;
        public Transform AntContainer;
        public string LevelScene;

        private int _antNumber;

        protected virtual void Start()
        {
            StartCoroutine(LoadLevel());
        }

        private void SpawnAnts(Vector3 anthillPosition)
        {
            const int antCount = 20;
            const float radius = 6f;

            for (var i = 0; i < antCount; i++)
            {
                var angle = i*2*Mathf.PI/antCount;
                var offset = new Vector3(
                    radius*Mathf.Cos(angle),
                    0f,
                    radius*Mathf.Sin(angle));
                var rotation = Quaternion.LookRotation(offset);
                var position = offset + anthillPosition;
                var obj = Instantiate(AntPrefab);
                obj.transform.parent = AntContainer;
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.name = string.Format("ant {0}", _antNumber++);
                var ant = obj.GetComponent<Ant>();
                ant.AnthillPosition = anthillPosition;
            }

        }

        private IEnumerator LoadLevel()
        {
            var asyncOperation = SceneManager.LoadSceneAsync(LevelScene, LoadSceneMode.Additive);

            while (!asyncOperation.isDone)
            {
                yield return new WaitForEndOfFrame();
            }

            var scene = SceneManager.GetSceneByName(LevelScene);
            var rootGameObjects = scene.GetRootGameObjects();

            var anthills = rootGameObjects.SelectMany(root => root.GetComponentsInChildren<AntHill>());

            foreach (var anthill in anthills)
            {
                SpawnAnts(anthill.transform.position);
            }
        }
    }
}
