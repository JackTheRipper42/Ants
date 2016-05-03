using MoonSharp.Interpreter;
using UnityEngine;

namespace Assets.Scripts.Lua
{
    public class MarkTable : IMarkTable
    {
        public MarkTable(Mark mark, Ant ant)
        {
            information = mark.Information;
            var diff = mark.transform.position - ant.transform.position;
            direction = -Mathf.Atan2(diff.z, diff.x)*Mathf.Rad2Deg;
            direction -= ant.transform.rotation.eulerAngles.y;
        }

        public Table information { get; private set; }

        public float direction { get; private set; }
    }
}
