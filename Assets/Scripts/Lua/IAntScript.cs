using MoonSharp.Interpreter;
using UnityEngine;

namespace Assets.Scripts.Lua
{
    public interface IAntScript
    {
        // ReSharper disable InconsistentNaming

        DynValue antEnterView { get; set; }

        DynValue antExitView { get; set; }

        DynValue appleEnterView { get; set; }

        DynValue appleExitView { get; set; }

        DynValue leaveApple { get; set; }

        DynValue leaveSugar { get; set; }

        DynValue reachApple { get; set; }

        DynValue reachSugar { get; set; }

        DynValue reachAnthill { get; set; }

        DynValue reachBoundaries { get; set; }

        DynValue reachDestination { get; set; }

        DynValue sugarEnterView { get; set; }

        DynValue sugarExitView { get; set; }

        DynValue reachMark { get; set; }

        DynValue update { get; set; }

        Vector2 position { get; }

        float rotation { get; }

        bool isCarrying { get; }

        void setDestinationGlobal(float x, float y);

        void setDestination(float distance, float orientation);

        void goToAnthill();

        bool pickSugar();

        bool pickApple();

        void mark(float radius, Table information);

        // ReSharper restore InconsistentNaming
    }
}