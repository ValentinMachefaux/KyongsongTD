using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text waveText;
    public TMP_Text levelText;

    void Update()
    {
        if (GameManager.Instance != null)
        {
            scoreText.text = "Score : " + GameManager.Instance.score;
            waveText.text = "Wave : " + GameManager.Instance.currentWave;
            levelText.text = "Level : " + GameManager.Instance.currentLevel;
        }
    }
}
