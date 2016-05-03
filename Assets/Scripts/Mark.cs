using System;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using MoonSharp.Interpreter;
using UnityEngine;

namespace Assets.Scripts
{
    public class Mark : MonoBehaviour
    {
        private const float MinSize = 5f;
        private const float MaxAge = 10f;

        private float _age;
        private float _maxAge;
        private float _radius;
        private bool _initialized;

        public Table Information { get; private set; }

        public Ant Creator { get; private set; }

        public void Initialize(float radius, [CanBeNull] Table information, [NotNull] Ant creator)
        {
            if (creator == null)
            {
                throw new ArgumentNullException("creator");
            }
            if (radius <= 0)
            {
                throw new ArgumentOutOfRangeException("radius", radius, "The radius is less or equal to zero.");
            }

            _initialized = true;
            _radius = radius;
            Information = information;
            Creator = creator;
            _age = 0f;
            _maxAge = MaxAge*MinSize/(MinSize + radius);
        }

        protected virtual void Update()
        {
            if (!_initialized)
            {
                return;
            }

            _age += Time.deltaTime;

            var currentRadius = _radius * _age / _maxAge;
            transform.localScale = new Vector3(currentRadius, transform.transform.localScale.y, currentRadius);

            if (_age >= _maxAge)
            {
                Destroy(gameObject);
            }
        }
    }
}
