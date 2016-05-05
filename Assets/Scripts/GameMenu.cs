using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameMenu : MonoBehaviour
    {
        public GameObject MenuBar;
        public GameObject MenuPanel;
        public Dropdown SpeedDropdown;
        public Text PointsText;

        private Parameters _parameters;
        private GameManager _gameManager;
        private float[] _speedValues;

        public void MenuClicked()
        {
            Time.timeScale = 0f;
            MenuPanel.SetActive(true);
        }

        public void MainMenuClicked()
        {
            SceneManager.LoadScene(0);
        }

        public void ExitClicked()
        {
            Application.Quit();
        }

        public void ContinueClicked()
        {
            MenuPanel.SetActive(false);
            Time.timeScale = _parameters.TimeScale;
        }

        public void SpeedValueChanged(int index)
        {
            var timeScale = _speedValues[index];
            _parameters.TimeScale = timeScale;
            Time.timeScale = timeScale;
        }

        protected virtual void Start()
        {
            _parameters = FindObjectOfType<Parameters>();
            _gameManager = FindObjectOfType<GameManager>();
            _gameManager.PointsChanged += GameManagerOnPointsChanged;
            SetPoints(_gameManager.Points);

            _speedValues = new[] { 1 / 8f, 1 / 4f, 1 / 2f, 1f, 2f, 4f, 8f };
            SpeedDropdown.options = new List<Dropdown.OptionData>
            {
                new Dropdown.OptionData("1/8x"),
                new Dropdown.OptionData("1/4x"),
                new Dropdown.OptionData("1/2x"),
                new Dropdown.OptionData("1x"),
                new Dropdown.OptionData("2x"),
                new Dropdown.OptionData("4x"),
                new Dropdown.OptionData("8x")
            };

            var speedIndex = 0;
            var minDiff = float.MaxValue;
            for (int i = 0; i < _speedValues.Length; i++)
            {
                var diff = Mathf.Abs(_parameters.TimeScale - _speedValues[i]);
                if (diff < minDiff)
                {
                    minDiff = diff;
                    speedIndex = i;
                }
            }
            SpeedDropdown.value = speedIndex;
            _parameters.TimeScale = _speedValues[speedIndex];
            Time.timeScale = _parameters.TimeScale;

            MenuBar.SetActive(true);
            MenuPanel.SetActive(false);
        }

        protected virtual void OnDestroy()
        {
            _gameManager.PointsChanged -= GameManagerOnPointsChanged;
        }

        private void GameManagerOnPointsChanged(object sender, EventArgs eventArgs)
        {
            SetPoints(_gameManager.Points);
        }

        private void SetPoints(int value)
        {
            PointsText.text = value.ToString();
        }
    }
}
