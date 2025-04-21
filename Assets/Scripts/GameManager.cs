using UnityEngine;
using UnityEngine.SceneManagement;  // Pour gérer les scènes
using TMPro;
using Script;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;  // Instance unique pour le Singleton

    private Base playerBase;

    public int score = 0; 
    public int currentWave = 1;
    public int currentLevel = 1; 
    public int scoreToNextLevel = 100;

    public float waveCooldown = 20f;
    private float waveTimer = 0f;
    public TMP_Text waveCountText; 

    public bool isGameOver = false;

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
    // Si le jeu n'est pas terminé, gérer la progression des vagues
    if (!isGameOver)
    {
        waveTimer += Time.deltaTime;  // Compter le temps écoulé

        // Vérifie si le cooldown est écoulé et lance la prochaine vague
        if (waveTimer >= waveCooldown)
        {
            StartNextWave();  // Lancer la vague suivante
            waveTimer = 0f;   // Réinitialiser le timer
        }

        // Afficher le compte à rebours avant la vague suivante
        if (waveCountText != null)
        {
            float timeLeft = Mathf.Max(0f, waveCooldown - waveTimer);  // Temps restant avant la vague
            waveCountText.text = "Wave " + currentWave + " in " + Mathf.CeilToInt(timeLeft) + "s";
        }

        // Mettre à jour l'UI
        UpdateUI();
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