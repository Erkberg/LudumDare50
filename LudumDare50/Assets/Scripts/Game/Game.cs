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
        public EnemyController enemyPrefab;
        public EnemyController currentEnemy;
        public Cam cam;
        public Transform worldHolder;

        private void Awake()
        {
            inst = this;
        }

        public void OnGameStarted()
        {
            player.isActive = true;
            StartCoroutine(GoToNextEnemySequence());
        }

        public void OnPlayerDeath()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void OnEnemyDeath()
        {
            state.OnEnemyKilled();
            NextEnemy();
        }

        public void OnEnemyExhaust()
        {
            NextEnemy();
        }

        public void NextEnemy()
        {
            state.enemyLevel++;
            StartCoroutine(GoToNextEnemySequence());
        }

        private IEnumerator GoToNextEnemySequence()
        {     
            // Spawn next enemy
            float nextEnemyPositionX = (state.enemyLevel + 1) * 12;
            Vector3 nextEnemyPosition = new Vector3(nextEnemyPositionX, 0f, 0f);
            EnemyController enemy = Instantiate(enemyPrefab, nextEnemyPosition, Quaternion.identity, worldHolder);
            enemy.InitWithStats(state.GetNextEnemyStats());            

            // Move player and camera
            float nextPlayerPositionX = nextEnemyPositionX - 4f;
            float moveSpeed = 4f;
            player.SetMoveSpeed(moveSpeed);
            cam.SetMoveSpeed(moveSpeed);
            yield return new WaitUntil(() => player.transform.position.x >= nextPlayerPositionX);
            player.SetMoveSpeed(0f);
            player.ChangeState(0);
            cam.SetMoveSpeed(0f);
            ui.SetStoryTextByLevel(state.enemyLevel);

            currentEnemy = enemy;
            enemy.isActive = true;
        }
    }
}
