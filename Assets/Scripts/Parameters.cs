using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class Parameters : MonoBehaviour
    {
        public string AntScriptName { get; set; }

        public List<string> AntScripts { get; private set; } 

        public float TimeScale { get; set; }

        protected virtual void Start()
        {
            DontDestroyOnLoad(this);
            var files = Directory.GetFiles(Application.streamingAssetsPath, "*.lua");
            AntScripts = files.Select(file => new FileInfo(file).Name).ToList();
            AntScriptName = AntScripts.First();
            TimeScale = 1f;
        }
    }
}
