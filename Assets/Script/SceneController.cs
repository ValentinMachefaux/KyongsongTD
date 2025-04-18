using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void LoadScene(int index, LoadSceneMode mode = LoadSceneMode.Single)
    {
        SceneManager.LoadScene(index, mode);
    }
}