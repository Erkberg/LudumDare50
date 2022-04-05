using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LudumDare50
{
    public class Health : MonoBehaviour
    {
        public Image fillImage;
        public float maxHealth = 100f;
        public float currentHealth;
        public ParticleSystem damageParticle;

        public Action onDeath;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        public void SetMaxHealth(float max)
        {
            maxHealth = max;
            currentHealth = max;
        }

        public void ChangeHealth(float amount)
        {
            currentHealth += amount;
            if (currentHealth > maxHealth)
                currentHealth = maxHealth;
            else if(currentHealth <= 0f)
            {
                currentHealth = 0f;
                onDeath?.Invoke();
            }

            UpdateHealthUI();

            if (amount < 0f)
            {
                Game.inst.cam.screenshake.StartShake(amount / 100f, 0.1f);
                if(damageParticle)
                    damageParticle.Emit((int)(-amount / 6));
            }                
        }

        public void UpdateHealthUI()
        {
            fillImage.fillAmount = currentHealth / maxHealth;
        }
    }
}
