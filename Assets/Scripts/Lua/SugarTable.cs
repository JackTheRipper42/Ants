using UnityEngine;

namespace Assets.Scripts.Lua
{
    public class SugarTable : EntityTable, ISugarTable
    {
        private readonly Sugar _sugar;

        public SugarTable(Sugar sugar)
            :base(sugar)
        {
            _sugar = sugar;
        }

        public int mount
        {
            get { return _sugar.Capacity; }
        }
    }
}
