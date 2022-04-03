using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ErksUnityLibrary;

namespace LudumDare50
{
    public class EnemyAnimation : MonoBehaviour
    {
        public Animator animator;
        public Animator fadeAnimator;
        public Animator humanAnimator;
        public FlickerRenderers flickerRenderers;
        public SkinnedMeshRenderer meshRenderer;
        public SkinnedMeshRenderer fadeMeshRenderer;
        public SkinnedMeshRenderer humanMeshRenderer;

        public Material blackFadeMaterial;
        public Material redFadeMaterial;
        public Material whiteFadeMaterial;

        private const string StateIntName = "state";
        private const string StateChangeTriggerName = "stateChange";

        public void SetState(int stateId)
        {
            animator.SetInteger(StateIntName, stateId);

            if (stateId != 0)
                animator.SetTrigger(StateChangeTriggerName);
        }

        public void OnTakeDamage()
        {
            flickerRenderers.StartFlicker(0.167f);
        }

        public void OnLoseEndurance()
        {

        }

        public IEnumerator FadeToDeadHumanSequence()
        {
            fadeAnimator.gameObject.SetActive(true);
            fadeAnimator.Play("metarig|Dead");
            humanAnimator.gameObject.SetActive(true);
            humanAnimator.Play("metarig|Dead");
            humanMeshRenderer.materials[0].SetColorA(0f);
            humanMeshRenderer.materials[1].SetColorA(0f);
            animator.gameObject.SetActive(false);
            float alpha = 1f;
            while(alpha > 0f)
            {
                alpha -= Time.deltaTime / 4f;
                fadeMeshRenderer.materials[0].SetColorA(alpha);
                fadeMeshRenderer.materials[1].SetColorA(alpha);
                humanMeshRenderer.materials[0].SetColorA(1f - alpha);
                humanMeshRenderer.materials[1].SetColorA(1f - alpha);
                yield return null;
            }
        }
    }
}
