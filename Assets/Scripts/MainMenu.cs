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
        public InputField AntScriptNameInputField;

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

        public void EndEditingScriptName(string scriptName)
        {
            _parameters.AntScriptName = scriptName;
        }

        protected virtual void Start()
        {
            _parameters = FindObjectOfType<Parameters>();
            MainMenuPanel.SetActive(true);
            OptionsMenuPanel.SetActive(false);
            AntScriptNameInputField.text = _parameters.AntScriptName;
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
