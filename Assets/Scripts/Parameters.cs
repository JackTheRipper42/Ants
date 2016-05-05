using System.IO;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class Parameters : MonoBehaviour
    {
        public string AntScriptName { get; set; }

        public string[] AntScripts { get; private set; } 

        protected virtual void Start()
        {
            DontDestroyOnLoad(this);
            var files = Directory.GetFiles(Application.streamingAssetsPath, "*.lua");
            AntScripts = files.Select(file => new FileInfo(file).Name).ToArray();
            AntScriptName = AntScripts.First();
        }
    }
}
