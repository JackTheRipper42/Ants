using System.Runtime.InteropServices;
using MoonSharp.Interpreter;
using UnityEngine;

namespace Assets.Scripts
{
    public class Mark : MonoBehaviour
    {
        private const float MinSize = 5f;
        private const float MaxAge = 10f;

        //private float _maxRadius;
        //private float _radius;
        //private float _decay;
        //private float _remainingTime;
        private float _age;
        private float _maxAge;
        private float _radius;
        private bool _initialized;

        public Table Information { get; private set; }

        public void Initialize(float radius, Table information)
        {
            _initialized = true;
            Information = information;
            _radius = radius;
            _age = 0f;
            _maxAge = MaxAge*MinSize/(MinSize + radius);

            //_maxRadius = radius;
            //_decay = 10;
            //_radius = 0.01f;
            //_remainingTime = 5000f/radius;
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

            //_radius += Time.deltaTime*10f;
            //_radius = Mathf.Min(_maxRadius, _radius);
            //_remainingTime -= Time.deltaTime*_decay;

            //transform.localScale = new Vector3(_radius, transform.transform.localScale.y, _radius);

            //if (_remainingTime <= 0)
            //{
            //    Destroy(gameObject);
            //}

            //if (_radius > 0)
            //{
            //    transform.localScale = new Vector3(_radius, transform.transform.localScale.y, _radius);
            //    _radius -= Time.deltaTime*_decay;
            //}
            //else
            //{
            //    Destroy(gameObject);
            //}
        }
    }
}
