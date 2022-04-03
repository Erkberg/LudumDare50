using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ErksUnityLibrary;

namespace LudumDare50
{
    public class EnemyAnimation : MonoBehaviour
    {
        public Animator animator;
        public FlickerRenderers flickerRenderers;

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
    }
}
