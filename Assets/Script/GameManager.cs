using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int playerMoney = 100;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddMoney(int amount)
    {
        playerMoney += amount;
        Debug.Log("Money: " + playerMoney);
        // Tu peux ici mettre à jour l’UI plus tard
    }
}
