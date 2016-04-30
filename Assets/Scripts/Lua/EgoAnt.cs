using JetBrains.Annotations;
using MoonSharp.Interpreter;
using System;
using UnityEngine;

namespace Assets.Scripts.Lua
{
    public class EgoAnt : IEgoAnt
    {
        private readonly Ant _ant;

        public EgoAnt([NotNull] Ant ant)
        {
            if (ant == null)
            {
                throw new ArgumentNullException("ant");
            }

            _ant = ant;
        }

        public Vector2 position
        {
            get { return ToVector2(_ant.transform.position); }
        }

        public bool isCarrying
        {
            get { return _ant.HasSugar; }
        }

        public DynValue update { get; set; }

        public DynValue antEnterView { get; set; }

        public DynValue sugarEnterView { get; set; }

        public DynValue appleEnterView { get; set; }

        public DynValue antExitView { get; set; }

        public DynValue sugarExitView { get; set; }

        public DynValue appleExitView { get; set; }

        public DynValue reachSugar { get; set; }

        public DynValue reachApple { get; set; }

        public DynValue reachAnthill { get; set; }

        public DynValue leaveSugar { get; set; }

        public DynValue leaveApple { get; set; }

        public void setDestinationGlobal(float x, float y)
        {
            _ant.SetDestination(ToVector3(x, y));
        }

        public void setDestinationLocal(float x, float y)
        {
            _ant.SetDestination(_ant.transform.position + ToVector3(x, y));
        }

        public void setDestination(float direction, float orientation)
        {
            var destination = _ant.transform.rotation*Quaternion.Euler(0f, orientation, 0f)*
                              new Vector3(direction, 0f, 0f)
                              + _ant.transform.position;
            _ant.SetDestination(destination);
        }

        public void goToAnthill()
        {
            _ant.SetDestination(_ant.AnthillPosition);
        }

        public bool pickSugar()
        {
            return _ant.PickSugar();
        }

        private Vector3 ToVector3(float x, float z)
        {
            return new Vector3(x, _ant.transform.position.y, z);
        }

        private static Vector2 ToVector2(Vector3 value)
        {
            return new Vector2(value.x, value.z);
        }
    }
}
