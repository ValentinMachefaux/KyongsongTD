using UnityEngine;
using UnityEngine.SceneManagement;  // Pour gérer les scènes
using TMPro;
using Script;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;  // Instance unique pour le Singleton

    private Base playerBase;

    public int score = 0; 
    public int currentWave = 0;
    public int currentLevel = 1; 
    public int scoreToNextLevel = 100;

    public float waveCooldown = 30f;
    private float waveTimer = 0f;
    public TMP_Text waveCountText; 

    public bool isGameOver = false;

    private bool firstWaveStarted = false;

    // UI Elements (pour afficher le score etc)
    public TMP_Text scoreText;
    public TMP_Text levelText;
    public TMP_Text waveText;
    public TMP_Text gameOverText;


    private void Awake()
    {
        // Gérer le Singleton - Si une autre instance existe, la détruire
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Persister entre les scènes
        }
        else
        {
            Destroy(gameObject);  // Détruire l'objet si une autre instance existe
        }
    }


    private void Update()
    {
        if (!isGameOver && Base.isBasePlaced)
        {
            waveTimer += Time.deltaTime;

            if (waveTimer >= waveCooldown)
            {
                StartNextWave();
                waveTimer = 0f;
            }

            if (waveCountText != null)
            {
                float timeLeft = Mathf.Max(0f, waveCooldown - waveTimer);
                waveCountText.text = "Wave " + currentWave + " in " + Mathf.CeilToInt(timeLeft) + "s";
            }

            UpdateUI();
        }
        else if (waveCountText != null)
        {
            waveCountText.text = "Click to place the Base"; // N'affiche rien tant que la base n'est pas posée
        }
    }


    public void AddScore(int points)
    {
        if (!isGameOver)
        {
            score += points;
            if (score >= scoreToNextLevel)
            {
                LevelUp();
            }
        }
    }

    // Méthode pour passer au niveau suivant
    private void LevelUp()
    {
        currentLevel++;
        scoreToNextLevel += 100;  // Augmenter le score requis pour le prochain niveau

        Debug.Log("Niveau " + currentLevel + " atteint !");
    }

    // Méthode pour démarrer la prochaine vague
    public void StartNextWave()
    {
        if (!isGameOver)
        {
            currentWave++;
            Debug.Log("Vague " + currentWave + " commencée !");

            // Augmenter la difficulté, par exemple, augmenter le nombre d'ennemis ou leur vitesse
            // (Cela pourrait être une fonction qui change les paramètres du jeu selon la vague)

            // Tu peux aussi ajouter des événements comme une augmentation de la difficulté ou un délai avant la prochaine vague
            GameObject baseGO = GameObject.FindGameObjectWithTag("Base"); // Trouve la base
                if (baseGO == null)
                {
                    Debug.LogWarning("Base introuvable pour le spawn des ennemis.");
                    return;
                }

                Base baseScript = baseGO.GetComponent<Base>();
                if (baseScript == null || baseScript.enemyPrefab == null)
                {
                    Debug.LogWarning("Composant Base ou prefab ennemi manquant.");
                    return;
                }

                int enemiesToSpawn = 1 + (currentWave - 1) * 2;

                for (int i = 0; i < enemiesToSpawn; i++)
                {
                    Vector2 randomCircle = Random.insideUnitCircle.normalized * baseScript.spawnRadius;
                    Vector3 spawnPosition = baseGO.transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);

                    GameObject enemyGO = Instantiate(baseScript.enemyPrefab, spawnPosition, Quaternion.identity);
                    Enemy enemy = enemyGO.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.Initialize(baseGO);
                    }
                }
        }
    }

    // Méthode pour finir le jeu
    public void GameOver()
    {
        isGameOver = true;
        gameOverText.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        score = 0;
        currentWave = 1;
        currentLevel = 1;
        waveCooldown = 5f;
        isGameOver = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Recharger la scène actuelle
    }

    // Mettre à jour l'UI avec les informations actuelles
    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;

        if (levelText != null)
            levelText.text = "Level : " + currentLevel;

        if (waveText != null)
            waveText.text = "Wave : " + currentWave;

        if (gameOverText != null && isGameOver)
            gameOverText.text = "GAME OVER!";

    }
}