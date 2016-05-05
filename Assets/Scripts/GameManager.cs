using MoonSharp.Interpreter;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        private const float FoodSpawnSpace = 15f;

        public GameObject AntPrefab;
        public GameObject MarkPrefab;
        public GameObject ApplePrefab;
        public GameObject SugarPrefab;
        public string LevelScene;

        private int _antNumber;
        private int _markNumber;
        private int _sugarNumber;
        private int _appleNumber;
        private Level _level;
        private Parameters _parameters;
        private List<Anthill> _anthills;

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
            _parameters = FindObjectOfType<Parameters>();
            StartCoroutine(LoadLevel());
        }

        private void SpawnAnts(Anthill anthill)
        {
            const int antCount = 20;
            const float radius = 6f;

            for (var i = 0; i < antCount; i++)
            {
                var angle = i*360/antCount;
                var offset = Quaternion.Euler(0f, angle, 0f)*Vector3.forward*radius;
                var obj = Instantiate(AntPrefab);
                obj.transform.parent = anthill.AntContainer;
                obj.transform.position = offset + anthill.transform.position;
                obj.transform.rotation = Quaternion.LookRotation(offset);
                obj.name = string.Format("ant {0}", _antNumber++);
                var ant = obj.GetComponent<Ant>();
                ant.Initialize(anthill.transform.position, this, _parameters.AntScriptName);
            }
        }

        private void SpawnApple(Vector3 position)
        {
            var obj = Instantiate(ApplePrefab);
            obj.transform.position = position;
            obj.transform.parent = _level.FoodContainer;
            obj.transform.name = string.Format("apple {0}", _appleNumber++);
        }

        private void SpawnSugar(Vector3 position)
        {
            var obj = Instantiate(SugarPrefab);
            obj.transform.position = position;
            obj.transform.parent = _level.FoodContainer;
            obj.transform.name = string.Format("sugar {0}", _sugarNumber++);
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

            _level = rootGameObjects.SelectMany(root => root.GetComponentsInChildren<Level>()).First();
            LevelBoundaries = _level.GetLevelBounds();
            _anthills = rootGameObjects.SelectMany(root => root.GetComponentsInChildren<Anthill>()).ToList();

            StartCoroutine(SpawnSugarRoutine());
            StartCoroutine(SpawnApplesRoutine());

            foreach (var anthill in _anthills)
            {
                SpawnAnts(anthill);
            }
        }

        private IEnumerator SpawnSugarRoutine()
        {
            for (int i = 0; i < 4; i++)
            {
                Vector3 position;
                if (TryGetRandomPosition(10, out position))
                {
                    SpawnSugar(position);
                }
            }

            while (true)
            {
                yield return new WaitForSeconds(Random.Range(10, 60));
                Vector3 position;
                if (TryGetRandomPosition(10, out position))
                {
                    SpawnSugar(position);
                }
            }
        }

        private IEnumerator SpawnApplesRoutine()
        {
            for (int i = 0; i < 3; i++)
            {
                Vector3 position;
                if (TryGetRandomPosition(10, out position))
                {
                    SpawnApple(position);
                }
            }

            while (true)
            {
                yield return new WaitForSeconds(Random.Range(30, 90));
                Vector3 position;
                if (TryGetRandomPosition(10, out position))
                {
                    SpawnApple(position);
                }
            }
        }

        private bool TryGetRandomPosition(int maxIterations, out Vector3 position)
        {
            var iterations = -1;
            do
            {
                if (iterations++ >= maxIterations)
                {
                    position = new Vector3();
                    return false;
                }
                position = new Vector3(
                    Random.Range(LevelBoundaries.xMin + FoodSpawnSpace, LevelBoundaries.xMax - FoodSpawnSpace),
                    0f,
                    Random.Range(LevelBoundaries.yMin + FoodSpawnSpace, LevelBoundaries.yMax - FoodSpawnSpace));

            } while (!IsPositionValid(position));
            return true;
        }

        private static bool IsPositionValid(Vector3 position)
        {
            var colliders = Physics.OverlapSphere(position, FoodSpawnSpace);
            foreach (var collider in colliders)
            {
                var anthill = collider.gameObject.GetComponent<Anthill>();
                var ant = collider.gameObject.GetComponent<Ant>();
                var sugar = collider.gameObject.GetComponent<Sugar>();
                var apple = collider.gameObject.GetComponent<Apple>();

                if (anthill != null || ant != null || sugar != null || apple != null)
                {
                    return false;
                }
            }

            return true;
        }
    }
}