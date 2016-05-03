using MoonSharp.Interpreter;
using UnityEngine;

namespace Assets.Scripts.Lua
{
    public interface IMarkTable
    {
        // ReSharper disable InconsistentNaming

        float direction { get; }

        Table information { get; }

        // ReSharper restore InconsistentNaming
    }
}