using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LudumDare50
{
    public class Endurance : MonoBehaviour
    {
        public Image fillImage;
        public float maxEndurance = 100f;
        public float currentEndurance;

        public Action onExhaust;

        private void Awake()
        {
            currentEndurance = maxEndurance;
        }

        public void SetMaxEndurance(float max)
        {
            maxEndurance = max;
            currentEndurance = max;
        }

        public void ChangeEndurance(float amount)
        {
            currentEndurance += amount;
            if (currentEndurance > maxEndurance)
                currentEndurance = maxEndurance;
            else if (currentEndurance <= 0f)
            {
                currentEndurance = 0f;
                onExhaust?.Invoke();
            }

            UpdateEnduranceUI();
        }

        public void UpdateEnduranceUI()
        {
            fillImage.fillAmount = currentEndurance / maxEndurance;
        }
    }
}
