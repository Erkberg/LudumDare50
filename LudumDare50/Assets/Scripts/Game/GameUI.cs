using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LudumDare50
{
    public class GameUI : MonoBehaviour
    {
        public TextMeshProUGUI levelText;
        public TextMeshProUGUI perfectText;
        public List<PlayerStateButton> playerStateButtons;

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

        public void SetLevel(int level)
        {
            levelText.text = level.ToString();
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
    }
}
