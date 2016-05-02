using UnityEngine;

namespace Assets.Scripts.Lua
{
    public class AntTable : EntityTable, IAntTable
    {
        public AntTable(Ant ant) : base(ant)
        {
        }
    }
}
