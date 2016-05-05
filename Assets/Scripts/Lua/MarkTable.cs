using MoonSharp.Interpreter;
using UnityEngine;

namespace Assets.Scripts.Lua
{
    public class MarkTable : IMarkTable
    {
        public MarkTable(Mark mark, Ant ant)
        {
            information = mark.Information;
            var globalDirection = Quaternion.LookRotation(mark.transform.position - ant.transform.position);
            direction = globalDirection.eulerAngles.y - ant.transform.rotation.eulerAngles.y;
        }

        public Table information { get; private set; }

        public float direction { get; private set; }
    }
}
