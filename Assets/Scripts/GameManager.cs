using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Script;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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

    public TMP_Text scoreText;
    public TMP_Text levelText;
    public TMP_Text waveText;
    public TMP_Text gameOverText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject); // Persiste entre les scènes
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Évite les doublons si une autre instance existe
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
            waveCountText.text = "Click to place the Base";
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

    private void LevelUp()
    {
        currentLevel++;
        scoreToNextLevel += 100;
        Debug.Log("Niveau " + currentLevel + " atteint !");
    }

    public void StartNextWave()
    {
        if (!isGameOver)
        {
            currentWave++;
            Debug.Log("Vague " + currentWave + " commencée !");

            GameObject baseGO = GameObject.FindGameObjectWithTag("Base");
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

    public void GameOver()
    {
        isGameOver = true;
        if (gameOverText != null)
            gameOverText.gameObject.SetActive(true);
    }
    
    public void GoToMenu()
    {
        ResetGameValues();
        SceneManager.LoadScene(0); // Retour menu
    }
    

    public void ResetGameValues()
    {
        score = 0;
        currentWave = 0;
        currentLevel = 1;
        scoreToNextLevel = 100;
        waveCooldown = 30f;
        waveTimer = 0f;
        isGameOver = false;
        Base.isBasePlaced = false;
    }

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
