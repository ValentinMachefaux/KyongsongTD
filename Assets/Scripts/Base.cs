using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace Script
{
    public class Base : MonoBehaviour
    {
        public float health = 100f;
        public float startDelay = 0f;
        public static bool isBasePlaced = false;
        public TMPro.TMP_Text countdownText;  // Texte du compte à rebours avant la vague
        private float countdownTimer;
        
        public GameObject enemyPrefab;
        public Transform baseTarget;
        public float spawnRadius = 1f;
        public float spawnInterval = 3f;

        private float timer;

        void Start()
        {
            countdownTimer = startDelay;  
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
                    isBasePlaced = true;
                    
                    if (countdownText != null)
                    {
                        countdownText.gameObject.SetActive(false); // Cacher le texte du compte à rebours
                    }
                    else
                    {
                        Debug.LogWarning("countdownText is not assigned!"); // Avertissement si countdownText est null
                    }
                }
                return; 
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

    }
}