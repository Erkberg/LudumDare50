using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare50
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerState currentState;
        public PlayerAnimation playerAnimation;
        public Health health;
        public Endurance endurance;
        public List<PlayerStateData> stateData;

        private GameInput input;
        private float durationInCurrentState = 0f;

        private void Awake()
        {
            input = Game.inst.input;
            health.onDeath += OnDeath;
        }

        private void Update()
        {
            durationInCurrentState += Time.deltaTime;
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
            if (!Game.inst.currentEnemy && stateId != 0)
                return;

            PlayerState newState = (PlayerState)stateId;

            if(currentState == PlayerState.None || (currentState != PlayerState.None && stateId == 0))
            {
                currentState = newState;
                durationInCurrentState = 0f;
                playerAnimation.SetState(stateId);

                if(stateId != 0)
                    StartCoroutine(StateChangeSequence());
            }            
        }

        private IEnumerator StateChangeSequence()
        {
            if (currentState == PlayerState.Attack)
                StartCoroutine(AttackSequence());
            yield return new WaitForSeconds(GetDataByState(currentState).duration);
            ChangeState(0);
        }

        private IEnumerator AttackSequence()
        {
            yield return new WaitForSeconds(GetDataByState(currentState).durationTillPerfect);
            Attack();
        }

        private void Attack()
        {
            Game.inst.currentEnemy.OnGettingAttacked();
        }

        private void OnDeath()
        {
            Debug.Log("you have died");
            Game.inst.OnPlayerDeath();
        }

        private PlayerStateData GetDataByState(PlayerState state)
        {
            return stateData.Find(x => x.state == state);
        }
    }
}
