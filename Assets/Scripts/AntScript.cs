using System;
using MoonSharp.Interpreter;
using System.IO;
using Assets.Scripts.Lua;
using UnityEngine;

namespace Assets.Scripts
{
    public class AntScript : IAntScript
    {
        private readonly Script _script;
        private readonly Ant _ant;

        public AntScript(Ant ant)
        {
            _ant = ant;

            UserData.RegisterType<Vector2>();
            UserData.RegisterType<AntTable>();
            UserData.RegisterType<SugarTable>();
            UserData.RegisterType<AppleTable>();
            UserData.RegisterType<IAntScript>();

            var scriptPath = Path.Combine(Application.streamingAssetsPath, "ant.lua");
            var scriptCode = File.ReadAllText(scriptPath);

            _script = new Script(CoreModules.Preset_HardSandbox);
            _script.Options.DebugPrint = Debug.Log;
            _script.DoString(scriptCode);
            _script.Globals["ant"] = this;

            const string initFunctionName = "init";
            var initFunction = _script.Globals.Get(initFunctionName);
            if (initFunction == null || initFunction.IsNil())
            {
                throw new InvalidOperationException(string.Format(
                    "The function '{0}' is not found in script '{1}'.",
                    initFunctionName,
                    scriptPath));
            }
            _script.Call(initFunction);
        }

        public Vector2 position
        {
            get { return ToVector2(_ant.transform.position); }
        }

        public bool isCarrying
        {
            get { return _ant.IsCarrying; }
        }

        public DynValue update { get; set; }

        public DynValue antEnterView { get; set; }

        public DynValue sugarEnterView { get; set; }

        public DynValue appleEnterView { get; set; }

        public DynValue antExitView { get; set; }

        public DynValue sugarExitView { get; set; }

        public DynValue appleExitView { get; set; }

        public DynValue reachSugar { get; set; }

        public DynValue reachApple { get; set; }

        public DynValue reachAnthill { get; set; }

        public DynValue reachDestination { get; set; }

        public DynValue leaveSugar { get; set; }

        public DynValue leaveApple { get; set; }

        public void setDestinationGlobal(float x, float y)
        {
            _ant.SetDestination(ToVector3(x, y));
        }

        public void setDestination(float distance, float direction)
        {
            _ant.SetDestination(distance, direction);
        }

        public void goToAnthill()
        {
            _ant.SetDestination(_ant.AnthillPosition);
        }

        public bool pickSugar()
        {
            return _ant.PickSugar();
        }

        public bool pickApple()
        {
            return _ant.PickApple();
        }

        public void Update()
        {
            CallLuaFunction(update);
        }

        public void EnterView(Ant ant)
        {
            CallLuaFunction(antEnterView, new AntTable(ant));
        }

        public void EnterView(Sugar sugar)
        {
            CallLuaFunction(sugarEnterView, new SugarTable(sugar));
        }

        public void EnterView(Apple apple)
        {
            CallLuaFunction(appleEnterView, new AppleTable(apple));
        }

        public void ExitView(Ant ant)
        {
            CallLuaFunction(antExitView, new AntTable(ant));
        }

        public void ExitView(Sugar sugar)
        {
            CallLuaFunction(sugarExitView, new SugarTable(sugar));
        }

        public void ExitView(Apple apple)
        {
            CallLuaFunction(appleExitView, new AppleTable(apple));
        }

        public void Reach(Sugar sugar)
        {
            CallLuaFunction(reachSugar, new SugarTable(sugar));
        }

        public void Reach(Apple apple)
        {
            CallLuaFunction(reachApple, new AppleTable(apple));
        }

        public void Reach(AntHill antHill)
        {
            CallLuaFunction(reachAnthill);
        }

        public void ReachDestination()
        {
            CallLuaFunction(reachDestination);
        }

        public void Leave(Sugar sugar)
        {
            CallLuaFunction(leaveSugar, new SugarTable(sugar));
        }

        public void Leave(Apple apple)
        {
            CallLuaFunction(leaveApple, new AppleTable(apple));
        }

        private Vector3 ToVector3(float x, float z)
        {
            return new Vector3(x, _ant.transform.position.y, z);
        }

        private static Vector2 ToVector2(Vector3 value)
        {
            return new Vector2(value.x, value.z);
        }

        private void CallLuaFunction(DynValue function, params object[] args)
        {
            if (function != null && !function.IsNil())
            {
                _script.Call(function, args);
            }
        }
    }
}
