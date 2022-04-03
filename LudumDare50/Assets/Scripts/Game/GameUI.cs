using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ErksUnityLibrary;

namespace LudumDare50
{
    public class GameUI : MonoBehaviour
    {
        public static bool bottomUiEnabled = true;

        public GameObject storyTextBackground;
        public TextMeshProUGUI storyText;
        public TextMeshProUGUI perfectText;
        public GameMenu menu;
        public List<PlayerStateButton> playerStateButtons;
        public List<TextById> storyTexts;
        public GameObject bottomUiHolder;
        public Image actionAvailableImage;

        public GameObject textjd;
        public GameObject textda;

        private void Awake()
        {
            textda.SetActive(bottomUiEnabled);
            textjd.SetActive(bottomUiEnabled);
        }

        private void Update()
        {
            HandleActionImage();
        }

        private void HandleActionImage()
        {
            if(!bottomUiEnabled)
            {
                SetActionAvailableImageAlpha(0.0625f);
                return;
            }

            bool perfectAttackAvailable = Game.inst.player.IsPerfectAttackAvailable();            
            bool actionAvailable = Game.inst.player.IsActionAvailable();
            bool perfectDefenceAvailable = false;
            if(Game.inst.currentEnemy)
            {
                perfectDefenceAvailable = Game.inst.currentEnemy.perfectDefenceAvailable;
            }

            if (perfectAttackAvailable || (perfectDefenceAvailable && actionAvailable))
            {
                SetActionAvailableImageAlpha(1f);
            }
            else if(actionAvailable)
            {
                SetActionAvailableImageAlpha(0.5f);
            }
            else
            {
                SetActionAvailableImageAlpha(0.0625f);
            }
        }

        public void OnIndicatorClicked()
        {
            bottomUiEnabled = !bottomUiEnabled;
            textda.SetActive(bottomUiEnabled);
            textjd.SetActive(bottomUiEnabled);
        }

        private void SetActionAvailableImageAlpha(float alpha)
        {
            Color color = actionAvailableImage.color;
            color.a = alpha;
            actionAvailableImage.color = color;
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

        public void SetBottomUiActive(bool active)
        {
            bottomUiHolder.SetActive(active);
            /*foreach (PlayerStateButton button in playerStateButtons)
            {
                button.gameObject.SetActive(active);
            }*/
        }

        public void SetStoryTextByLevel(int level)
        {
            SetStoryText(storyTexts.Find(x => x.id == level).text);
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

        public void OnGameEnd()
        {
            menu.Open(GameMenu.State.End);
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
            SetStoryText(text);
        }

        public void ClearStoryText()
        {
            SetStoryText(string.Empty);
        }

        private void SetStoryText(string s)
        {
            storyText.text = s;

            storyTextBackground.SetActive(!s.Equals(string.Empty));
        }
    }
}
