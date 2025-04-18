using UnityEngine;

namespace Script
{
    public class Base : MonoBehaviour
    {
        public GameObject enemyPrefab;
        public Transform baseTarget;
        public float spawnRadius = 5f;
        public float spawnInterval = 3f;

        private float timer;

        void Update()
        {
            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                SpawnEnemy();
                timer = 0f;
            }
        }

        void SpawnEnemy()
        {
            if (enemyPrefab == null)
            {
                Debug.LogWarning("EnemyPrefab non assigné.");
                return;
            }

            Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnRadius;
            Vector3 spawnPosition = transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);

            GameObject enemyGO = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            Enemy enemy = enemyGO.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Initialize(this.gameObject);
            }
        }
    }
}