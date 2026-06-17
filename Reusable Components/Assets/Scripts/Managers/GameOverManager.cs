using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GameOver()
    {
        Debug.Log("GAME OVER");
        Instantiate(Instance, transform.position, Quaternion.identity);
    }

}
