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
        public List<PlayerStateButton> playerStateButtons;
        public List<TextById> storyTexts;

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

        public void OnQuitButtonClicked()
        {
            Application.Quit();
        }
    }
}
