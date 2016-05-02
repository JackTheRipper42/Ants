using UnityEngine;

namespace Assets.Scripts.Lua
{
    public interface ISugarTable
    {
        // ReSharper disable InconsistentNaming

        int mount { get; }

        Vector2 position { get; }

        // ReSharper restore InconsistentNaming
    }
}