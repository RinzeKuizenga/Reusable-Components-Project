using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;

public class FightSetup : MonoBehaviour
{
    public int level;
    [SerializeField] private Enemy enemyPrefab;

    Vector2 enemySpawnPos = new Vector2(10, 0);

    private void Start()
    {
        level = GameProgress.Instance.level;

        Debug.Log("FightSetup level: " + level);

        SpawnEnemies();
        FindObjectOfType<BattleManager>().SetupBattle();
        PlaylistPlayer.Instance.NextMusic();
        StartCoroutine(StartGameCoroutine());
    }

    IEnumerator StartGameCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        FindObjectOfType<BattleManager>().StartCurrentTurn();
    }

    void SpawnEnemies()
    {
        int enemyAmount = CalculateEnemyAmount();

        for(int i = 0; i < enemyAmount; i++)
        {
            Enemy currentEnemy = Instantiate(enemyPrefab, enemySpawnPos, Quaternion.identity);
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
