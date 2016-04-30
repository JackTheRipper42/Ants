using MoonSharp.Interpreter;
using UnityEngine;

namespace Assets.Scripts.Lua
{
    public interface IEgoAnt
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

        DynValue reachDestination { get; set; }

        DynValue sugarEnterView { get; set; }

        DynValue sugarExitView { get; set; }

        DynValue update { get; set; }

        Vector2 position { get; }

        bool isCarrying { get; }

        void setDestinationGlobal(float x, float y);

        void setDestinationLocal(float x, float y);

        void setDestination(float direction, float orientation);

        void goToAnthill();

        bool pickSugar();

        // ReSharper restore InconsistentNaming
    }
}