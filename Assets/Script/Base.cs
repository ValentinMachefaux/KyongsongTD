using UnityEngine;

namespace Script
{
    public class Base : MonoBehaviour
    {
        public float health = 10f;
        
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
        
        public void TakeDamage(float amount)
        {
            health -= amount;
            if (health <= 0f)
            {
                Die();
            }
        }

        // Méthode pour détruire la base
        private void Die()
        {
            Destroy(gameObject);  // Détruire l'objet lorsque la base n'a plus de points de vie
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