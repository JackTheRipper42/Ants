using System.Collections;
using System.Linq;
using MoonSharp.Interpreter;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public GameObject AntPrefab;
        public GameObject MarkPrefab;
        public string LevelScene;

        private int _antNumber;
        private int _markNumber;

        public Rect LevelBoundaries { get; private set; }

        public void SpawnMark(
            Ant creator,
            float radius,
            Table information)
        {
            var obj = Instantiate(MarkPrefab);
            obj.transform.position = creator.transform.position;
            obj.transform.parent = creator.transform.parent;
            obj.transform.name = string.Format("mark {0}", _markNumber++);
            var mark = obj.GetComponent<Mark>();
            mark.Initialize(radius, information, creator);
        }

        protected virtual void Start()
        {
            StartCoroutine(LoadLevel());
        }

        private void SpawnAnts(Transform anthill)
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
                var position = offset + anthill.position;
                var obj = Instantiate(AntPrefab);
                obj.transform.parent = anthill;
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.name = string.Format("ant {0}", _antNumber++);
                var ant = obj.GetComponent<Ant>();
                ant.Initialize(anthill.position, this);
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

            var level = rootGameObjects.SelectMany(root => root.GetComponentsInChildren<Level>()).First();
            LevelBoundaries = level.GetLevelBounds();

            var anthills = rootGameObjects.SelectMany(root => root.GetComponentsInChildren<AntHill>());
            foreach (var anthill in anthills)
            {
                SpawnAnts(anthill.transform);
            }
        }
    }
}
