using MoonSharp.Interpreter;
using UnityEngine;

namespace Assets.Scripts.Lua
{
    public class EgoAnt
    {
        public Vector2 position { get; set; }

        public DynValue update { get; set; }

        public DynValue antEnterView { get; set; }

        public DynValue sugarEnterView { get; set; }

        public DynValue appleEnterView { get; set; }

        public DynValue antExitView { get; set; }

        public DynValue sugarExitView { get; set; }

        public DynValue appleExitView { get; set; }

        public DynValue reachSugar { get; set; }

        public DynValue reachApple { get; set; }

        public DynValue leaveSugar { get; set; }

        public DynValue leaveApple { get; set; }
    }
}
