using UnityEngine;

public class GameOver : MonoBehaviour
{
    public void OnReplayButton()
    {
        GameManager.Instance?.ResetGameValues();
    }

    public void OnMenuButton()
    {
        GameManager.Instance?.GoToMenu();
    }
}