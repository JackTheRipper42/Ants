using MoonSharp.Interpreter;
using System.IO;
using Assets.Scripts.Lua;
using UnityEngine;

namespace Assets.Scripts
{
    public class AntScript
    {
        private readonly Script _script;
        private readonly EgoAnt _egoAnt;

        public AntScript()
        {
            UserData.RegisterType<Vector2>();
            UserData.RegisterType<AntTable>();
            UserData.RegisterType<SugarTable>();
            UserData.RegisterType<AppleTable>();
            UserData.RegisterType<EgoAnt>();

            var scriptPath = Path.Combine(Application.streamingAssetsPath, "ant.lua");

            _script = new Script(CoreModules.Preset_HardSandbox);
            _script.Options.DebugPrint = Debug.Log;

            var scriptCode = File.ReadAllText(scriptPath);
            _script.DoString(scriptCode);

            _egoAnt = new EgoAnt();
            _script.Globals["ant"] = _egoAnt;

            var initFunction = _script.Globals.Get("init");
            _script.Call(initFunction);
        }

        public void Update()
        {
            CallLuaFunction(_egoAnt.update);
        }

        public void EnterView(Ant ant)
        {
            CallLuaFunction(_egoAnt.antEnterView, new AntTable(ant));
        }

        public void EnterView(Sugar sugar)
        {
            CallLuaFunction(_egoAnt.sugarEnterView, new SugarTable(sugar));
        }

        public void EnterView(Apple apple)
        {
            CallLuaFunction(_egoAnt.appleEnterView, new AppleTable(apple));
        }

        public void ExitView(Ant ant)
        {
            CallLuaFunction(_egoAnt.antExitView, new AntTable(ant));
        }

        public void ExitView(Sugar sugar)
        {
            CallLuaFunction(_egoAnt.sugarExitView, new SugarTable(sugar));
        }

        public void ExitView(Apple apple)
        {
            CallLuaFunction(_egoAnt.appleExitView, new AppleTable(apple));
        }

        public void Reach(Sugar sugar)
        {
            CallLuaFunction(_egoAnt.reachSugar, new SugarTable(sugar));
        }

        public void Reach(Apple apple)
        {
            CallLuaFunction(_egoAnt.reachApple, new AppleTable(apple));
        }

        public void Leave(Sugar sugar)
        {
            CallLuaFunction(_egoAnt.leaveSugar, new SugarTable(sugar));
        }

        public void Leave(Apple apple)
        {
            CallLuaFunction(_egoAnt.leaveApple, new AppleTable(apple));
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
