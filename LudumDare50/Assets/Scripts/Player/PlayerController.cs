using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ErksUnityLibrary;

namespace LudumDare50
{
    public class PlayerController : MonoBehaviour
    {
        public bool isActive;
        public PlayerState currentState;
        public PlayerAnimation playerAnimation;
        public Health health;
        public Endurance endurance;
        public List<PlayerStateData> stateData;

        private GameInput input;
        private float durationInCurrentState = 0f;
        private float moveSpeed;
        private bool perfectActionPossible;
        private bool perfectAttack;

        private void Awake()
        {
            input = Game.inst.input;
            health.onDeath += OnDeath;
        }

        private void Update()
        {
            if (!isActive)
                return;

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

                if (perfectActionPossible)
                {
                    perfectAttack = true;                    
                }                    
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
            if ((!Game.inst.currentEnemy && stateId != 0))
                return;

            PlayerState newState = (PlayerState)stateId;

            if(CanChangeState(stateId))
            {
                StopAllCoroutines();
                currentState = newState;
                durationInCurrentState = 0f;
                playerAnimation.SetState(stateId);
                //Debug.Log("change state to " + stateId);
                Game.inst.ui.OnPlayerStateChanged(currentState);

                if(stateId != 0)
                    StartCoroutine(StateChangeSequence());
            }            
        }

        private bool CanChangeState(int newStateId)
        {
            bool switchToIdle = currentState != PlayerState.None && newStateId == 0;
            bool animationCancel = currentState != PlayerState.None && 
                durationInCurrentState > GetDataByState(currentState).durationTillCancelPossible;
            return switchToIdle || animationCancel || currentState == PlayerState.None;
        }

        private IEnumerator StateChangeSequence()
        {
            if (currentState == PlayerState.Attack)
                StartCoroutine(AttackSequence());
            PlayerStateData stateData = GetDataByState(currentState);
            StartCoroutine(PerfectActionCheckSequence(stateData));
            yield return new WaitForSeconds(stateData.duration);
            ChangeState(0);
        }

        private IEnumerator PerfectActionCheckSequence(PlayerStateData stateData)
        {
            yield return new WaitForSeconds(stateData.durationTillPerfect);
            perfectActionPossible = true;
            yield return new WaitForSeconds(stateData.perfectDuration);
            perfectActionPossible = false;
        }

        // returns perfect action
        public bool OnGettingAttacked(AttackType attackType, float damage)
        {
            bool dodged = (attackType == AttackType.Low && currentState == PlayerState.Jump) ||
                (attackType == AttackType.Mid && currentState == PlayerState.Defend) ||
                (attackType == AttackType.High && currentState == PlayerState.Duck);

            if(!dodged)
            {
                health.ChangeHealth(-damage);
                playerAnimation.OnTakeDamage();
            }
            else
            {
                if (perfectActionPossible)
                {
                    Game.inst.ui.OnPerfectAction(currentState);
                    return true;
                }                    
            }

            return false;
        }

        private IEnumerator AttackSequence()
        {
            yield return new WaitForSeconds(GetDataByState(currentState).durationTillPerfect);
            Attack();
        }

        private void Attack()
        {
            Game.inst.currentEnemy.OnGettingAttacked(perfectAttack);
            if (perfectAttack)
            {
                perfectAttack = false;
                Game.inst.ui.OnPerfectAction(PlayerState.Attack);
            }            
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
