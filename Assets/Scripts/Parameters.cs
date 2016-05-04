using UnityEngine;

namespace Assets.Scripts
{
    public class Parameters : MonoBehaviour
    {
        public string AntScriptName { get; set; }

        protected virtual void Start()
        {
            DontDestroyOnLoad(this);
            AntScriptName = "ant.lua";
        }
    }
}
