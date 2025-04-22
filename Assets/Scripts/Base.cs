using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.XR.ARFoundation;

namespace Script
{
    public class Base : MonoBehaviour
    {
        [Header("Base Settings")]
        public float health = 100f;
        public float startDelay = 0f;
        public static bool isBasePlaced = false;

        [Header("UI")]
        public TMP_Text countdownText;

        [Header("Enemy Settings")]
        public GameObject enemyPrefab;
        public Transform baseTarget;
        public float spawnRadius = 1f;
        public float spawnInterval = 3f;

        [Header("AR Settings")]
        public bool hidePlanesAfterPlacement = false;

        private float countdownTimer;

        void Start()
        {
            countdownTimer = startDelay;
        }

        void Update()
        {
            if (!isBasePlaced)
            {
                HandleCountdown();
                return;
            }

            // Tu peux rajouter d'autres comportements ici une fois la base placée
        }

        void HandleCountdown()
        {
            countdownTimer -= Time.deltaTime;

            if (countdownText != null)
            {
                countdownText.text = "Next wave in: " + Mathf.CeilToInt(countdownTimer) + "s";
            }

            if (countdownTimer <= 0f)
            {
                PlaceBase();
            }
        }

        void PlaceBase()
        {
            isBasePlaced = true;

            if (countdownText != null)
            {
                countdownText.gameObject.SetActive(false);
            }

            ARPlaneManager planeManager = FindObjectOfType<ARPlaneManager>();
            if (planeManager != null)
            {
                planeManager.enabled = false; // Arrête la détection de nouveaux plans

                if (hidePlanesAfterPlacement)
                {
                    foreach (ARPlane plane in planeManager.trackables)
                    {
                        plane.gameObject.SetActive(false); // Cache les plans existants si demandé
                    }
                }
            }

            Debug.Log("Base placed. AR Plane Detection stopped.");
        }

        public void TakeDamage(float amount)
        {
            health -= amount;
            Debug.Log("Vie restante : " + health);

            if (health <= 0f)
            {
                Die();
            }
        }

        void Die()
        {
            SceneManager.LoadScene(2); // tu changes de scène
            Destroy(gameObject);   
        }
    }
}
