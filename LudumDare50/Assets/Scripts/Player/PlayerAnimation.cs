using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare50
{
    public class PlayerAnimation : MonoBehaviour
    {
        public Animator animator;

        private const string StateIntName = "state";
        private const string StateChangeTriggerName = "stateChange";

        public void SetState(int stateId)
        {
            animator.SetInteger(StateIntName, stateId);

            if(stateId != 0)
                animator.SetTrigger(StateChangeTriggerName);
        }
    }
}
