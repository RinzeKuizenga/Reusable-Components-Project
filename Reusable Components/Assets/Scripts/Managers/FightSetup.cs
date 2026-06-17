using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public class FightSetup : MonoBehaviour
{
    public int level;
    [SerializeField] private Enemy enemyprefab;

    private void Start()
    {
        Debug.Log("FightSetup spawning enemies");
        SpawnEnemies();
        FindObjectOfType<BattleManager>().SetupBattle();
        PlaylistPlayer.Instance.NextMusic();
    }

    void SpawnEnemies()
    {
        int enemyAmount = CalculateEnemyAmount();

        for(int i = 0; i < enemyAmount; i++)
        {
            Enemy currentEnemy = Instantiate(enemyprefab, transform.position, Quaternion.identity);
            currentEnemy.level = level + 3;
            currentEnemy.enemyIndex = i;
        }
    }

    int CalculateEnemyAmount()
    {
        int enemyChance = UnityEngine.Random.Range(level, 51);

        if (enemyChance <= 25) return 1;
        if (enemyChance <= 40) return 2;
        return 3;
    }

}
