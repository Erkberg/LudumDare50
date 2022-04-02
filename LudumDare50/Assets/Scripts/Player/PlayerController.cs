using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ErksUnityLibrary;

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
        private float moveSpeed;

        private void Awake()
        {
            input = Game.inst.input;
            health.onDeath += OnDeath;
        }

        private void Update()
        {   
            if (moveSpeed.IsApproxEqual(0f))
            {                
                durationInCurrentState += Time.deltaTime;
                CheckInput();
            }      
            else
            {
                transform.AddPositionX(moveSpeed * Time.deltaTime);
            }
        }

        public void SetMoveSpeed(float speed)
        {
            moveSpeed = speed;
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
                Game.inst.ui.OnPlayerStateChanged(currentState);

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

        public void OnGettingAttacked(AttackType attackType, float damage)
        {
            bool dodged = (attackType == AttackType.Low && currentState == PlayerState.Jump) ||
                (attackType == AttackType.Mid && currentState == PlayerState.Defend) ||
                (attackType == AttackType.High && currentState == PlayerState.Duck);

            if(!dodged)
                health.ChangeHealth(-damage);
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
