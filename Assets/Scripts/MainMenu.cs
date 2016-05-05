using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class MainMenu : MonoBehaviour
    {
        public string SimulationScene;
        public GameObject MainMenuPanel;
        public GameObject OptionsMenuPanel;
        public Dropdown AntScriptNameDropdown;

        private Parameters _parameters;

        public void StartClicked()
        {
            StartCoroutine(LoadLevel());
        }

        public void OptionsClicked()
        {
            MainMenuPanel.SetActive(false);
            OptionsMenuPanel.SetActive(true);
        }

        public void ExitClicked()
        {
            Application.Quit();
        }

        public void ReturnClicked()
        {
            MainMenuPanel.SetActive(true);
            OptionsMenuPanel.SetActive(false);
        }

        public void ScriptSelected(int index)
        {
            _parameters.AntScriptName = _parameters.AntScripts[index];
        }

        protected virtual void Start()
        {
            _parameters = FindObjectOfType<Parameters>();
            MainMenuPanel.SetActive(true);
            OptionsMenuPanel.SetActive(false);
            AntScriptNameDropdown.options = _parameters.AntScripts
                .Select(file => new Dropdown.OptionData(file))
                .ToList();
            AntScriptNameDropdown.value = 0;
        }

        private IEnumerator LoadLevel()
        {
            var asyncOperation = SceneManager.LoadSceneAsync(SimulationScene, LoadSceneMode.Single);

            while (!asyncOperation.isDone)
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
