using UnityEngine;
using UnityEngine.SceneManagement;  // Pour gérer les scènes
using TMPro;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;  // Instance unique pour le Singleton

    public int score = 0; 
    public int currentWave = 1;
    public int currentLevel = 1; 
    public int scoreToNextLevel = 100;  // Score requis pour passer au niveau suivant

    public float waveCooldown = 5f;  // Délai entre les vagues (en secondes)
    private float waveTimer = 0f;

    public bool isGameOver = false;  // Si le jeu est terminé ou non

    // UI Elements (par exemple, pour afficher le score, le niveau et les vagues)
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
            waveTimer += Time.deltaTime;

            if (waveTimer >= waveCooldown)
            {
                StartNextWave();  // Lancer la vague suivante
                waveTimer = 0f;   // Réinitialiser le timer
            }

            // Mise à jour de l'UI
            UpdateUI();
        }
    }

    // Méthode pour ajouter des points
    public void AddScore(int points)
    {
        if (!isGameOver)
        {
            score += points;
            if (score >= scoreToNextLevel)
            {
                LevelUp();  // Passer au niveau suivant
            }
        }
    }

    // Méthode pour passer au niveau suivant
    private void LevelUp()
    {
        currentLevel++;
        scoreToNextLevel += 100;  // Augmenter le score requis pour le prochain niveau

        // Afficher un message (peut être amélioré avec une animation ou une UI)
        Debug.Log("Niveau " + currentLevel + " atteint !");
    }

    // Méthode pour démarrer la prochaine vague
    private void StartNextWave()
    {
        currentWave++;
        Debug.Log("Vague " + currentWave + " commencée !");

        // Augmenter la difficulté, par exemple, augmenter le nombre d'ennemis ou leur vitesse
        // (Cela pourrait être une fonction qui change les paramètres du jeu selon la vague)

        // Tu peux aussi ajouter des événements comme une augmentation de la difficulté ou un délai avant la prochaine vague
    }

    // Méthode pour finir le jeu
    public void GameOver()
    {
        isGameOver = true;
        gameOverText.gameObject.SetActive(true);  // Afficher un message de game over
    }

    // Méthode pour réinitialiser le jeu (par exemple, après un game over)
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
            levelText.text = "Niveau: " + currentLevel;

        if (waveText != null)
            waveText.text = "Vague: " + currentWave;

        if (gameOverText != null && isGameOver)
            gameOverText.text = "GAME OVER!";

    }
}