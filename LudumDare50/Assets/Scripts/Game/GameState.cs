using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare50
{
    public class GameState : MonoBehaviour
    {
        public int enemyLevel = 0;
        public List<EnemyStats> enemyStats;

        public EnemyStats GetNextEnemyStats()
        {
            return enemyStats.Find(x => x.level == enemyLevel);
        }
    }
}
