using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LudumDare50
{
    public class GameUI : MonoBehaviour
    {
        public TextMeshProUGUI storyText;
        public TextMeshProUGUI perfectText;
        public GameObject titleScreen;
        public GameObject tutorialScreen;
        public GameObject endScreen;
        public GameObject pauseScreen;
        public List<PlayerStateButton> playerStateButtons;
        public List<TextById> storyTexts;

        private void Update()
        {
            if (Game.inst.input.GetMenuDown())
                CheckTogglePauseScreen();
        }

        private void CheckTogglePauseScreen()
        {
            if (titleScreen.activeSelf || endScreen.activeSelf || tutorialScreen.activeSelf)
                return;

            if(pauseScreen.activeSelf)
            {
                pauseScreen.SetActive(false);
                Time.timeScale = 1f;
            }
            else
            {
                pauseScreen.SetActive(true);
                Time.timeScale = 0f;
            }
        }

        public void OnPlayerStateChanged(PlayerState state)
        {
            if(state == PlayerState.None)
            {
                SetAllPlayerStateButtonsInteractable(true);
            }
            else
            {
                SetAllPlayerStateButtonsInteractable(false);
                playerStateButtons.Find(x => x.state == state).SetInteractable(true);
            }
        }

        private void SetAllPlayerStateButtonsInteractable(bool interactable)
        {
            foreach(PlayerStateButton button in playerStateButtons)
            {
                button.SetInteractable(interactable);
            }
        }

        public void SetStoryTextByLevel(int level)
        {
            storyText.text = storyTexts.Find(x => x.id == level).text;
        }

        public void OnPerfectAction(PlayerState state)
        {
            StartCoroutine(PerfectActionSequence());
        }

        private IEnumerator PerfectActionSequence()
        {
            perfectText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.33f);
            perfectText.gameObject.SetActive(false);
        }

        public void OnStartButtonClicked()
        {
            titleScreen.SetActive(false);
            Game.inst.OnGameStarted();
        }

        public void OnTutorialButtonClicked()
        {
            titleScreen.SetActive(false);
            tutorialScreen.SetActive(true);
        }

        public void OnTutorialBackButtonClicked()
        {
            titleScreen.SetActive(true);
            tutorialScreen.SetActive(false);
        }

        public void OnGameEnd()
        {
            endScreen.SetActive(true);
        }

        public void SetStoryTextByEnding(GameState.EndState endState)
        {
            string text = string.Empty;
            switch (endState)
            {
                case GameState.EndState.Pacifist:
                    text = storyTexts.Find(x => x.id == 10).text;
                    break;

                case GameState.EndState.Attacked:
                    text = storyTexts.Find(x => x.id == 11).text;
                    break;

                case GameState.EndState.Killed:
                    text = storyTexts.Find(x => x.id == 12).text;
                    break;

                case GameState.EndState.KilledAll:
                    text = storyTexts.Find(x => x.id == 13).text;
                    break;
            }
            storyText.text = text;
        }

        public void ClearStoryText()
        {
            storyText.text = string.Empty;
        }

        public void OnQuitButtonClicked()
        {
            Application.Quit();
        }
    }
}
