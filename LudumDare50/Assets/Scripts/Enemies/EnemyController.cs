using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ErksUnityLibrary;

namespace LudumDare50
{
    public class EnemyController : MonoBehaviour
    {
        public bool isActive;
        public EnemyState currentState;
        public AttackType currentAttackType;
        public EnemyStats stats;
        public EnemyAnimation enemyAnimation;
        public Health health;
        public Endurance endurance;
        public bool perfectDefenceAvailable;

        private float actionTimePassed;
        private int attackStreak;
        private float idleWaitTime = 1f;
        private bool isIdle;

        private float perfectMultiplier = 3f;
        private float perfectDefenceWindow = 0.133f;

        private void Awake()
        {
            health.onDeath += OnDeath;
            endurance.onExhaust += OnExhaust;
        }

        public void InitWithStats(EnemyStats stats)
        {
            this.stats = stats;
            health.SetMaxHealth(stats.health);
            endurance.SetMaxEndurance(stats.endurance);
            gameObject.name = "Enemy_" + stats.level;
        }

        private void Update()
        {
            if(currentState == EnemyState.None && isActive)
            {
                
                if (attackStreak < stats.attackStreak)
                {
                    Timing.AddTimeAndCheckMax(ref actionTimePassed, stats.actionDelay, Time.deltaTime, Attack);
                }                    
                else
                {
                    if (!isIdle)
                        StartCoroutine(IdleSequence());
                }
            }            
        }

        private IEnumerator IdleSequence()
        {
            isIdle = true;
            yield return new WaitForSeconds(idleWaitTime);
            isIdle = false;
            attackStreak = 0;
        }

        private void Attack()
        {
            actionTimePassed = 0f;
            StartCoroutine(AttackSequence());            
        }

        private IEnumerator AttackSequence()
        {
            currentState = EnemyState.Attack;
            AttackType attackType = stats.attackTypes.GetRandomItem();
            enemyAnimation.SetState((int)attackType + 1);
            yield return new WaitForSeconds(0.67f - perfectDefenceWindow);
            perfectDefenceAvailable = true;
            yield return new WaitForSeconds(perfectDefenceWindow);
            perfectDefenceAvailable = false;
            //Debug.Log(gameObject.name + " attacks with type " + attackType);
            bool perfectDodge = Game.inst.player.OnGettingAttacked(attackType, stats.damagePerAttack);
            yield return new WaitForSeconds(0.33f);
            attackStreak++;
            float enduranceDrain = perfectDodge ? stats.endurancePerAction * perfectMultiplier : stats.endurancePerAction;  
            currentState = EnemyState.None;
            enemyAnimation.SetState(0);
            endurance.ChangeEndurance(-enduranceDrain);
        }

        public void OnGettingAttacked(bool perfectAttack)
        {
            float healthDrain = perfectAttack ? stats.damageTaken * perfectMultiplier : stats.damageTaken;
            health.ChangeHealth(-healthDrain);
            enemyAnimation.OnTakeDamage();
            Game.inst.state.OnEnemyAttacked();
        }

        private void OnDeath()
        {
            StopAllCoroutines();
            Game.inst.OnEnemyDeath();
            DisableUI();
            StartCoroutine(DeathSequence());
        }

        private void OnExhaust()
        {
            StopAllCoroutines();
            Game.inst.OnEnemyExhaust();
            DisableUI();
            StartCoroutine(ExhaustSequence());
        }

        private void DisableUI()
        {
            health.gameObject.SetActive(false);
            endurance.gameObject.SetActive(false);
        }

        private IEnumerator DeathSequence()
        {
            enemyAnimation.SetState(4);
            isActive = false;
            yield return new WaitForSeconds(1f);
            StartCoroutine(enemyAnimation.FadeToDeadHumanSequence());
        }

        private IEnumerator ExhaustSequence()
        {
            enemyAnimation.SetState(5);
            isActive = false;
            yield return new WaitForSeconds(1f);
            StartCoroutine(enemyAnimation.FadeToExhaustedHumanSequence());
        }
    }
}
