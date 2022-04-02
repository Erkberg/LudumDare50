using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare50
{
    public class GameState : MonoBehaviour
    {
        public int enemyLevel = 0;
        public List<EnemyStats> enemyStats;
        public EndState endState;
        
        public enum EndState
        {
            Pacifist,
            Attacked,
            Killed
        }

        public void OnEnemyAttacked()
        {
            if (endState == EndState.Pacifist)
                endState = EndState.Attacked;
        }

        public void OnEnemyKilled()
        {
            endState = EndState.Killed;
        }

        public EnemyStats GetNextEnemyStats()
        {
            return enemyStats.Find(x => x.level == enemyLevel);
        }
    }
}
