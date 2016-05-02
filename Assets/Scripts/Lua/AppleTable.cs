using UnityEngine;

namespace Assets.Scripts.Lua
{
    public class AppleTable : EntityTable, IAppleTable
    {
        private readonly Apple _apple;

        public AppleTable(Apple apple)
            :base(apple)
        {
            _apple = apple;
        }

        public int carryingAnts
        {
            get { return _apple.CarryingAntsCount; }
        }
    }
}
