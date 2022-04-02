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

        private float actionTimePassed;
        private int attackStreak;
        private float idleWaitTime = 1f;
        private bool isIdle;

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
            yield return new WaitForSeconds(0.67f);
            //Debug.Log(gameObject.name + " attacks with type " + attackType);
            bool perfectDodge = Game.inst.player.OnGettingAttacked(attackType, stats.damagePerAttack);
            yield return new WaitForSeconds(0.33f);
            attackStreak++;
            float enduranceDrain = perfectDodge ? stats.endurancePerAction * 2 : stats.endurancePerAction;
            endurance.ChangeEndurance(-enduranceDrain);
            currentState = EnemyState.None;
            enemyAnimation.SetState(0);
        }

        public void OnGettingAttacked(bool perfectAttack)
        {
            float healthDrain = perfectAttack ? stats.damageTaken * 2 : stats.damageTaken;
            health.ChangeHealth(-healthDrain);
            Game.inst.state.OnEnemyAttacked();
        }

        private void OnDeath()
        {
            Debug.Log("enemy has died");
            Game.inst.OnEnemyDeath();
            Destroy(gameObject);
        }

        private void OnExhaust()
        {
            Debug.Log("enemy has exhausted");
            Game.inst.OnEnemyExhaust();
            Destroy(gameObject);
        }
    }
}
