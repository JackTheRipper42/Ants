﻿using MoonSharp.Interpreter;
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
        private Anthill _anthill;

        public Vector3 AnthillPosition
        {
            get { return _anthill.transform.position; }
        }

        public float Speed { get { return 5; } }

        public bool IsCarrying
        {
            get { return _carrying != Carrying.Noting; }
        }

        public Vector3 Destination { get; private set; }

        public bool Alive
        {
            get { return true; }
        }

        public void Initialize(Anthill anthill, GameManager gameManager, string scriptName)
        {
            _state = State.Idle;
            _anthill = anthill;
            _gameManager = gameManager;
            _nearSugar = new List<Sugar>();
            _nearApples = new List<Apple>();
            _antScript = new AntScript(this, scriptName);
        }

        public void SetDestination(Vector3 destination)
        {
            InitDestination(destination);
        }

        public void SetDestination(float distance, float direction)
        {
            transform.Rotate(new Vector3(0f, direction, 0f), Space.World);
            var destination = transform.forward*distance + transform.position;
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
            apple.AddCarryingAnt(this);
            _carrying = Carrying.Apple;
            return true;
        }

        public void RemoveCarriedApple()
        {
            _carrying = Carrying.Noting;
            InitDestination(Destination);
        }

        public void CreateMark(float radius, Table information)
        {
            _gameManager.SpawnMark(this, radius, information);
        }

        public void Kill()
        {
            _state = State.Dead;
            _anthill.AntDied();
            Destroy(gameObject);
        }

        protected virtual void Update()
        {
            if (!Alive)
            {
                return;
            }

            if (Time.deltaTime > 0f)
            {

                _nearSugar.RemoveAll(sugar => sugar == null);
                _nearApples.RemoveAll(apple => apple == null);

                switch (_state)
                {
                    case State.Idle:
                        break;
                    case State.Walking:
                        transform.rotation = Quaternion.LookRotation(Destination - _startPosition);
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
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (!Alive)
            {
                return;
            }

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
            if (!Alive)
            {
                return;
            }

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
            if (!Alive)
            {
                return;
            }

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
            var anthill = collision.gameObject.GetComponent<Anthill>();
            if (anthill != null)
            {
                switch (_carrying)
                {
                    case Carrying.Noting:
                        _antScript.Reach(anthill);
                        break;
                    case Carrying.Sugar:
                        anthill.CollectSugar();
                        _carrying = Carrying.Noting;
                        _antScript.Reach(anthill);
                        break;
                    case Carrying.Apple:
                        break;
                    default:
                        throw new NotSupportedException(string.Format(
                            "The carring state '{0}' is not supported.",
                            _carrying));
                }
            }
            var mark = collision.gameObject.GetComponent<Mark>();
            if (mark != null)
            {
                if (mark.Creator != this)
                {
                    _antScript.Reach(mark);
                }
            }
            var levelBoundary = collision.gameObject.GetComponentInParent<LevelBoundary>();
            if (levelBoundary != null)
            {
                _state = State.Idle;
                Destination = transform.position;
                _antScript.ReachBoundaries();
            }
        }

        protected virtual void OnCollisionExit(Collision collision)
        {
            if (!Alive)
            {
                return;
            }

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
            Walking,
            Dead
        }

        private enum Carrying
        {
            Noting,
            Sugar,
            Apple
        }
    }
}
