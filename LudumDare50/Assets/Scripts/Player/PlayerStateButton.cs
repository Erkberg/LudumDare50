using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LudumDare50
{
    public class PlayerStateButton : MonoBehaviour
    {
        public Button button;
        public PlayerState state;

        public void SetInteractable(bool interactable)
        {
            button.interactable = interactable;
        }

        private void Reset()
        {
            button = GetComponent<Button>();
        }
    }
}
