using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Apple : MonoBehaviour
    {
        public float SpeedPerAnt = 1.5f;

        private List<Ant> _carryingAnts;

        public int CarryingAntsCount
        {
            get { return _carryingAnts.Count; }
        }

        public void AddCarryingAnt(Ant ant)
        {
            _carryingAnts.Add(ant);
        }

        public void DestroyApple()
        {
            foreach (var ant in _carryingAnts)
            {
                ant.RemoveCarriedApple();
            }
            Destroy(gameObject);
        }

        protected virtual void Start()
        {
            _carryingAnts = new List<Ant>();
        }

        protected virtual void Update()
        {
            if (_carryingAnts.Count == 0)
            {
                return;
            }

            var combinedDestination = new Vector3(0f, 0f, 0f);
            var maxSpeed = float.MaxValue;
            var combindedSpeed = 0f;
            foreach (var ant in _carryingAnts)
            {
                combinedDestination += ant.Destination - transform.position;
                maxSpeed = Mathf.Min(maxSpeed, ant.Speed);
                combindedSpeed += SpeedPerAnt;
            }
            var speed = Mathf.Min(combindedSpeed, maxSpeed);
            combinedDestination.y = 0;
            var movement = combinedDestination.normalized*Time.deltaTime*speed;
            foreach (var ant in _carryingAnts)
            {
                ant.transform.position += movement;
            }
            transform.position += movement;
        }
    }
}
