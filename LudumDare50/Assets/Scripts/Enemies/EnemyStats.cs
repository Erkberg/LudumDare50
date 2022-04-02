using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare50
{
    [CreateAssetMenu]
    public class EnemyStats : ScriptableObject
    {
        public int level;
        public List<AttackType> attackTypes = new List<AttackType>
            { AttackType.Low, AttackType.Mid, AttackType.High };
        public float health = 100f;
        public float damagePerAttack = 20f;
        public float damageTaken = 20f;
        public float endurance = 100f;
        public float endurancePerAction = 10f;
        public float actionDelay = 1f;
        public int attackStreak = 1;
    }
}
