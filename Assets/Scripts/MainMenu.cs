using System;
using System.Collections;
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
            Show(Menu.Options);
        }

        public void ExitClicked()
        {
            Application.Quit();
        }

        public void OkButtonClicked()
        {
            _parameters.AntScriptName = _parameters.AntScripts[AntScriptNameDropdown.value];
            Show(Menu.Main);
        }

        public void CancelButtonClicked()
        {
            AntScriptNameDropdown.value = _parameters.AntScripts.IndexOf(_parameters.AntScriptName);
            Show(Menu.Main);
        }

        protected virtual void Start()
        {
            _parameters = FindObjectOfType<Parameters>();
            AntScriptNameDropdown.AddOptions(_parameters.AntScripts);
            AntScriptNameDropdown.value = _parameters.AntScripts.IndexOf(_parameters.AntScriptName);
            Show(Menu.Main);
        }

        private IEnumerator LoadLevel()
        {
            var asyncOperation = SceneManager.LoadSceneAsync(SimulationScene, LoadSceneMode.Single);

            while (!asyncOperation.isDone)
            {
                yield return new WaitForEndOfFrame();
            }
        }

        private void Show(Menu menu)
        {
            switch (menu)
            {
                case Menu.Main:
                    MainMenuPanel.SetActive(true);
                    OptionsMenuPanel.SetActive(false);
                    break;
                case Menu.Options:
                    MainMenuPanel.SetActive(false);
                    OptionsMenuPanel.SetActive(transform);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("menu", menu, null);
            }
        }

        private enum Menu
        {
            Main,
            Options
        }
    }
}
