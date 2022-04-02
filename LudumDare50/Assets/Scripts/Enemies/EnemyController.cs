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

        private float actionTimePassed = 0f;

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
                Timing.AddTimeAndCheckMax(ref actionTimePassed, stats.actionDelay, Time.deltaTime, TriggerAction);
            }            
        }

        private void TriggerAction()
        {
            actionTimePassed = 0f;
            if(Random.value < stats.attackChance)
                Attack();
        }

        private void Attack()
        {            
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
            float enduranceDrain = perfectDodge ? stats.endurancePerAction * 2 : stats.endurancePerAction;
            endurance.ChangeEndurance(-enduranceDrain);
            currentState = EnemyState.None;
            enemyAnimation.SetState(0);
        }

        public void OnGettingAttacked(bool perfectAttack)
        {
            float healthDrain = perfectAttack ? stats.damagePerAttack * 2 : stats.damagePerAttack;
            health.ChangeHealth(-healthDrain);
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
