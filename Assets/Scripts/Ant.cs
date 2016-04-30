using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class Ant : MonoBehaviour
    {
        public float Speed = 5f;

        private AntScript _antScript;
        private State _state;
        private Vector3 _destination;
        private Vector3 _startPosition;
        private float _lerpPosition;
        private float _lerpLength;
        private List<Sugar> _nearSugar;
        private List<Apple> _nearApples; 

        public Vector3 AnthillPosition { get; set; }
        
        public bool HasSugar { get; private set; }

        public void SetDestination(Vector3 destination)
        {
           _state = State.Walking;
            _destination = destination;
            _lerpPosition = 0;
            _lerpLength = (destination - transform.position).magnitude;
            _startPosition = transform.position;
        }

        public bool PickSugar()
        {
            if (HasSugar)
            {
                return false;
            }
            var sugar = _nearSugar.FirstOrDefault();
            if (sugar == null)
            {
                return false;
            }
            if (sugar.Capacity <= 0)
            {
                return false;
            }
            sugar.Capacity--;
            HasSugar = true;
            return true;
        }

        protected virtual void Start()
        {
            _nearSugar = new List<Sugar>();
            _nearApples = new List<Apple>();
            _state = State.Idle;
            _antScript = new AntScript(this);
        }

        protected virtual void Update()
        {
            switch (_state)
            {
                case State.Idle:
                    break;
                case State.Walking:
                    transform.rotation = Quaternion.LookRotation(_destination - transform.position);
                    _lerpPosition += (Speed*Time.deltaTime)/_lerpLength;
                    transform.position = Vector3.Lerp(_startPosition, _destination, _lerpPosition);
                    if (_lerpPosition >= 1)
                    {
                        _state = State.Idle;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            _antScript.Update();
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (!other.isTrigger)
            {
                var ant = other.gameObject.GetComponentInParent<Ant>();
                if (ant != null)
                {
                    _antScript.EnterView(ant);
                }
                var sugar = other.gameObject.GetComponentInParent<Sugar>();
                if (sugar != null)
                {
                    _antScript.EnterView(sugar);
                }
                var apple = other.gameObject.GetComponentInParent<Apple>();
                if (apple != null)
                {
                    _antScript.EnterView(apple);
                }
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (!other.isTrigger)
            {
                var ant = other.gameObject.GetComponentInParent<Ant>();
                if (ant != null)
                {
                    _antScript.ExitView(ant);
                }
                var sugar = other.gameObject.GetComponentInParent<Sugar>();
                if (sugar != null)
                {
                    _antScript.ExitView(sugar);
                }
                var apple = other.gameObject.GetComponentInParent<Apple>();
                if (apple != null)
                {
                    _antScript.ExitView(apple);
                }
            }
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            var sugar = collision.gameObject.GetComponentInParent<Sugar>();
            if (sugar != null)
            {
                _nearSugar.Add(sugar);
                _antScript.Reach(sugar);
            }
            var apple = collision.gameObject.GetComponentInParent<Apple>();
            if (apple != null)
            {
                _nearApples.Add(apple);
                _antScript.Reach(apple);
            }
            var anthill = collision.gameObject.GetComponentInParent<AntHill>();
            if (anthill != null)
            {
                if (HasSugar)
                {
                    anthill.CollectedSugar++;
                    HasSugar = false;
                }
                _antScript.Reach(anthill);
            }
        }

        protected virtual void OnCollisionExit(Collision collision)
        {
            var sugar = collision.gameObject.GetComponentInParent<Sugar>();
            if (sugar != null)
            {
                _nearSugar.Remove(sugar);
                _antScript.Leave(sugar);
            }
            var apple = collision.gameObject.GetComponentInParent<Apple>();
            if (apple != null)
            {
                _nearApples.Remove(apple);
                _antScript.Leave(apple);
            }
        }

        private enum State
        {
            Idle,
            Walking
        }
    }
}
