using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare50
{
    public class EnemyController : MonoBehaviour
    {
        public State currentState;
        public EnemyAnimation enemyAnimation;
        public Health health;
        public Endurance endurance;

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
            health.onDeath += OnDeath;
            endurance.onExhaust += OnExhaust;
        }

        private void OnDeath()
        {
            Debug.Log("enemy has died");
            Game.inst.OnEnemyDeath();
        }

        private void OnExhaust()
        {
            Debug.Log("enemy has exhausted");
            Game.inst.OnEnemyExhaust();
        }
    }
}
