using UnityEngine;

namespace Assets.Scripts.Lua
{
    public interface IAppleTable
    {
        // ReSharper disable InconsistentNaming

        int carryingAnts { get; }

        Vector2 position { get; }

        // ReSharper restore InconsistentNaming
    }
}