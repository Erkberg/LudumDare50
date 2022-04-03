using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LudumDare50
{
    public class GameMenu : MonoBehaviour
    {
        public GameObject holder;
        public State currentState;

        public GameObject thanksText;
        public GameObject startResumeButton;
        public GameObject tutorialScreen;

        public TextMeshProUGUI startResumeText;
        public TextMeshProUGUI tutorialRestartText;
        public TextMeshProUGUI volumeValueText;
        public Slider volumeSlider;

        public static bool restart;

        public enum State
        {
            Start,
            Pause,
            End
        }

        private void Awake()
        {
            if (!restart)
                Open(State.Start);
            else
            {
                restart = false;                
                Game.inst.OnGameStarted();
            }

            OnVolumeChanged(AudioListener.volume);
            volumeSlider.value = AudioListener.volume;
        }

        private void Update()
        {
            if (Game.inst.input.GetMenuDown())
                CheckTogglePauseScreen();
        }

        private void CheckTogglePauseScreen()
        {
            if (!IsOpen())
                Open(State.Pause);
            else if (currentState == State.Pause && !tutorialScreen.activeSelf)
                Close();
            else if (tutorialScreen.activeSelf)
                OnTutorialBackButtonClicked();
        }

        public bool IsOpen()
        {
            return holder.activeSelf || tutorialScreen.activeSelf;
        }

        public void Open(State state)
        {
            Game.inst.ui.SetBottomUiActive(false);
            currentState = state;
            holder.SetActive(true);
            Time.timeScale = 0f;

            switch (state)
            {
                case State.Start:
                    startResumeText.text = "Start";
                    break;

                case State.Pause:
                    startResumeText.text = "Resume";
                    break;

                case State.End:
                    thanksText.SetActive(true);
                    startResumeButton.SetActive(false);
                    tutorialRestartText.text = "Restart";
                    break;
            }
        }

        public void Close()
        {
            Game.inst.ui.SetBottomUiActive(true);
            holder.SetActive(false);
            Time.timeScale = 1f;
        }

        public void OnStartResumeButtonClicked()
        {
            Close();

            if(currentState == State.Start)
                Game.inst.OnGameStarted();
        }

        public void OnTutorialRestartButtonClicked()
        {
            if(currentState == State.End)
            {
                restart = true;
                Time.timeScale = 1f;
                Game.inst.OnRestartButtonPressed();
            }
            else
            {
                holder.SetActive(false);
                tutorialScreen.SetActive(true);
            }            
        }

        public void OnTutorialBackButtonClicked()
        {
            Open(currentState);
            tutorialScreen.SetActive(false);
        }

        public void OnVolumeChanged(float value)
        {
            AudioListener.volume = value;
            volumeValueText.text = $"{Mathf.RoundToInt(value * 100)} %";
        }

        public void OnQuitButtonClicked()
        {
            Application.Quit();
        }
    }
}
