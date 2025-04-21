using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace Script
{
    public class Base : MonoBehaviour
    {
        public float health = 100f;
        public float startDelay = 10f;
        public bool isBasePlaced = false;
        public TMPro.TMP_Text countdownText;  // Texte du compte à rebours avant la vague
        private float countdownTimer;
        
        public GameObject enemyPrefab;
        public Transform baseTarget;
        public float spawnRadius = 5f;
        public float spawnInterval = 3f;

        private float timer;

        void Start()
        {
            countdownTimer = startDelay;  // Initialisation du délai avant le spawn
        }

        void Update()
        {
            if (!isBasePlaced)
            {
                countdownTimer -= Time.deltaTime;  // Décompte avant la pose de la base

                if (countdownText != null)
                {
                    countdownText.text = "Next wave in: " + Mathf.CeilToInt(countdownTimer).ToString() + "s";
                }

                if (countdownTimer <= 0f)
                {
                    // La base est posée et les vagues commencent
                    isBasePlaced = true;
                    countdownText.gameObject.SetActive(false); // Cacher le texte du compte à rebours
                    GameManager.Instance.StartNextWave();  // Démarre les vagues
                }

                return; // Si la base n'est pas encore posée, ne rien faire d'autre
            }

            // Après la pose de la base, commence les vagues
            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                SpawnEnemy();  // Spawner un ennemi
                timer = 0f;
            }
        }

        
        public void TakeDamage(float amount)
        {
            Debug.Log("Vie restante : " + health);

            if (health <= 0f)
            {
                Die();
            }
            health -= amount;
        }
        
        private void Die()
        {

            // Détruire la base
            SceneManager.LoadScene(2);
            Destroy(gameObject);  // Détruire l'objet lorsque la base n'a plus de points de vie
        }

        void SpawnEnemy()
        {
            if (enemyPrefab == null)
            {
                Debug.LogWarning("EnemyPrefab non assigné.");
                return;
            }

            // nombre d'ennemis en fonction de la vague
            int enemiesToSpawn = 1 + (GameManager.Instance.currentWave - 1) * 2;

            for (int i = 0; i < enemiesToSpawn; i++)
            {
                // Générer une position aléatoire dans un cercle autour de la base
                Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnRadius;
                Vector3 spawnPosition = transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);

                // Créer l'ennemi
                GameObject enemyGO = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

                // Initialiser l'ennemi
                Enemy enemy = enemyGO.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.Initialize(this.gameObject);  // Lier l'ennemi à la base
                }
            }

        }
    }
}