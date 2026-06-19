using UnityEngine;

public class GameProgress : MonoBehaviour
{
    public static GameProgress Instance;

    public int level = 1;
    public int world = 1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void NextLevel(int maxLevel)
    {
        level++;

        if (level > maxLevel)
        {
            level = 1;
        }
    }

    public void ResetProgress()
    {
        level = 1;
        world = 1;
    }
}