using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare50
{
    public class PlayerController : MonoBehaviour
    {
        public State currentState;
        public PlayerAnimation playerAnimation;
        public Health health;
        public Endurance endurance;

        private GameInput input;

        public enum State
        {
            None,
            Attack,
            Defend,
            Jump,
            Duck
        }

        private void Awake()
        {
            input = Game.inst.input;
            health.onDeath += OnDeath;
        }

        private void Update()
        {
            CheckInput();
        }

        private void CheckInput()
        {
            if(input.GetAttackDown())
            {
                ChangeState(1);
            }
            else if (input.GetDefendDown())
            {
                ChangeState(2);
            }
            else if (input.GetJumpDown())
            {
                ChangeState(3);
            }
            else if (input.GetDuckDown())
            {
                ChangeState(4);
            }
        }

        public void ChangeState(int stateId)
        {
            currentState = (State)stateId;
        }

        private void OnDeath()
        {
            Debug.Log("you have died");
            Game.inst.OnPlayerDeath();
        }
    }
}
