using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ErksUnityLibrary;

namespace LudumDare50
{
    public class Game : MonoBehaviour
    {
        public static Game inst;

        public GameInput input;
        public GameState state;
        public GameUI ui;
        public PlayerController player;
        public EnemyController currentEnemy;
        public Screenshake screenshake;

        private void Awake()
        {
            inst = this;
        }

        public void OnPlayerDeath()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void OnEnemyDeath()
        {
            NextEnemy();
        }

        public void OnEnemyExhaust()
        {
            NextEnemy();
        }

        public void NextEnemy()
        {
            state.enemyLevel++;
        }
    }
}
