using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare50
{
    public class EnemyAnimation : MonoBehaviour
    {
        public Animator animator;

        private const string StateIntName = "state";

        public void SetState(int stateId)
        {
            animator.SetInteger(StateIntName, stateId);
        }
    }
}
