using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class Ant : MonoBehaviour
    {
        private GameManager _gameManager;
        private AntScript _antScript;
        private State _state;
        private Vector3 _startPosition;
        private float _lerpPosition;
        private float _lerpLength;
        private List<Sugar> _nearSugar;
        private List<Apple> _nearApples;
        private Carrying _carrying;
        private Apple _carriedApple;

        public Vector3 AnthillPosition { get; private set; }

        public float Speed { get { return 5; } }

        public bool IsCarrying
        {
            get { return _carrying != Carrying.Noting; }
        }

        public Vector3 Destination { get; private set; }

        public void Initialize(Vector3 anthillPosition, GameManager gameManager)
        {
            AnthillPosition = anthillPosition;
            _gameManager = gameManager;
        }

        public void SetDestination(Vector3 destination)
        {
            InitDestination(destination);
        }

        public void SetDestination(float distance, float direction)
        {
            var newOrientation = transform.rotation*Quaternion.Euler(0f, direction, 0f);
            var destination = newOrientation*new Vector3(distance, 0f, 0f) + transform.position;
            InitDestination(destination);
        }

        public bool PickSugar()
        {
            if (IsCarrying)
            {
                return false;
            }
            var sugar = _nearSugar.FirstOrDefault();
            if (sugar == null)
            {
                return false;
            }
            if (!sugar.PickSugar())
            {
                return false;
            }
            _carrying = Carrying.Sugar;
            return true;
        }

        public bool PickApple()
        {
            if (IsCarrying)
            {
                return false;
            }
            var apple = _nearApples.FirstOrDefault();
            if (apple == null)
            {
                return false;
            }
            _carriedApple = apple;
            apple.AddCarryingAnt(this);
            _carrying = Carrying.Apple;
            return true;
        }

        public void RemoveCarriedApple()
        {
            _carrying = Carrying.Noting;
            _carriedApple = null;
            InitDestination(Destination);
        }

        public void CreateMark(float radius, Table information)
        {
            _gameManager.SpawnMark(this, radius, information);
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
            _nearSugar.RemoveAll(sugar => sugar == null);
            _nearApples.RemoveAll(apple => apple == null);

            switch (_state)
            {
                case State.Idle:
                    break;
                case State.Walking:
                    var diff = Destination - transform.position;
                    var angle = Mathf.Atan2(diff.z, diff.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0f, -angle, 0f);
                    if (_carrying != Carrying.Apple)
                    {
                        _lerpPosition += (Speed*Time.deltaTime)/_lerpLength;
                        var newPosition = Vector3.Lerp(_startPosition, Destination, _lerpPosition);
                        var newPosition2D = new Vector2(newPosition.x, newPosition.z);
                        if (_gameManager.LevelBoundaries.Contains(newPosition2D))
                        {
                            transform.position = newPosition;
                            if (_lerpPosition >= 1)
                            {
                                _state = State.Idle;
                                _antScript.ReachDestination();
                            }
                        }
                        else
                        {
                            _state = State.Idle;
                            Destination = transform.position;
                            _antScript.ReachBoundaries();
                        }
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
                var ant = other.gameObject.GetComponent<Ant>();
                if (ant != null)
                {
                    _antScript.EnterView(ant);
                }
                var sugar = other.gameObject.GetComponent<Sugar>();
                if (sugar != null)
                {
                    _antScript.EnterView(sugar);
                }
                var apple = other.gameObject.GetComponent<Apple>();
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
                var ant = other.gameObject.GetComponent<Ant>();
                if (ant != null)
                {
                    _antScript.ExitView(ant);
                }
                var sugar = other.gameObject.GetComponent<Sugar>();
                if (sugar != null)
                {
                    _antScript.ExitView(sugar);
                }
                var apple = other.gameObject.GetComponent<Apple>();
                if (apple != null)
                {
                    _antScript.ExitView(apple);
                }
            }
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            var sugar = collision.gameObject.GetComponent<Sugar>();
            if (sugar != null)
            {
                _nearSugar.Add(sugar);
                _antScript.Reach(sugar);
            }
            var apple = collision.gameObject.GetComponent<Apple>();
            if (apple != null)
            {
                _nearApples.Add(apple);
                _antScript.Reach(apple);
            }
            var anthill = collision.gameObject.GetComponent<AntHill>();
            if (anthill != null)
            {
                switch (_carrying)
                {
                    case Carrying.Noting:
                        break;
                    case Carrying.Sugar:
                        anthill.CollectedSugar++;                       
                        break;
                    case Carrying.Apple:
                        if (_carriedApple != null)
                        {
                            anthill.CollectedApples++;
                            _carriedApple.DestroyApple();
                            _carriedApple = null;
                        }
                        break;
                    default:
                        throw new NotSupportedException(string.Format(
                            "The carring state '{0}' is not supported.",
                            _carrying));
                }
                _carrying = Carrying.Noting;
                _antScript.Reach(anthill);
            }
            var mark = collision.gameObject.GetComponent<Mark>();
            if (mark != null)
            {
                if (mark.Creator != this)
                {
                    _antScript.Reach(mark);
                }
            }
            var level = collision.gameObject.GetComponentInParent<Level>();
            if (level != null)
            {
                _state = State.Idle;
                Destination = transform.position;
                _antScript.ReachBoundaries();
            }
        }

        protected virtual void OnCollisionExit(Collision collision)
        {
            var sugar = collision.gameObject.GetComponent<Sugar>();
            if (sugar != null)
            {
                _nearSugar.Remove(sugar);
                _antScript.Leave(sugar);
            }
            var apple = collision.gameObject.GetComponent<Apple>();
            if (apple != null)
            {
                _nearApples.Remove(apple);
                _antScript.Leave(apple);
            }
            var mark = collision.gameObject.GetComponent<Mark>();
            if (mark != null)
            {
                if (mark.Creator != this)
                {
                    _antScript.Leave(mark);
                }
            }
        }

        private void InitDestination(Vector3 destination)
        {
            Debug.DrawLine(transform.position, destination, Color.red, 1f);

            _state = State.Walking;
            Destination = destination;
            _lerpPosition = 0;
            _lerpLength = (destination - transform.position).magnitude;
            _startPosition = transform.position;
        }

        private enum State
        {
            Idle,
            Walking
        }

        private enum Carrying
        {
            Noting,
            Sugar,
            Apple
        }
    }
}
