using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void OnMenuButton()
    {
        SceneManager.LoadScene(0); 
    }
}