using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare50
{
    public class GameState : MonoBehaviour
    {
        public int enemyLevel;
        public List<EnemyStats> enemyStats;
        public EndState endState;

        private int enemiesKilled;

        public enum EndState
        {
            Pacifist,
            Attacked,
            Killed,
            KilledAll
        }

        public void OnEnemyAttacked()
        {
            if (endState == EndState.Pacifist)
                endState = EndState.Attacked;
        }

        public void OnEnemyKilled()
        {
            endState = EndState.Killed;
            enemiesKilled++;

            if (enemiesKilled == enemyStats.Count)
                endState = EndState.KilledAll;
        }

        public EnemyStats GetNextEnemyStats()
        {
            return enemyStats.Find(x => x.level == enemyLevel);
        }

        public bool LastEnemyKilled()
        {
            return enemyLevel >= enemyStats.Count;
        }
    }
}
