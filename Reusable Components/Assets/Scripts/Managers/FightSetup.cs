using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;

public class FightSetup : MonoBehaviour
{
    // Stores the current game level used to scale enemy difficulty.
    public int level;

    // Enemy prefab that will be spawned at the start of the battle.
    [SerializeField] private Enemy enemyPrefab;

    // Position where all enemies are initially spawned.
    Vector2 enemySpawnPos = new Vector2(10, 0);

    private void Start()
    {
        // Get the current progression level from the persistent game progress system.
        level = GameProgress.Instance.level;

        Debug.Log("FightSetup level: " + level);

        // Create enemies before setting up the battle turn order.
        SpawnEnemies();

        // Set up battle participants after all enemies have been created.
        FindObjectOfType<BattleManager>().SetupBattle();

        // Start or continue the background music playlist.
        PlaylistPlayer.Instance.NextMusic();

        // Wait briefly so all battle components have time to initialize.
        StartCoroutine(StartGameCoroutine());
    }

    // Starts the first battle turn after the setup process has completed.
    IEnumerator StartGameCoroutine()
    {
        yield return new WaitForSeconds(0.1f);

        FindObjectOfType<BattleManager>().StartCurrentTurn();
    }

    // Spawns the calculated number of enemies and assigns their level and formation index.
    void SpawnEnemies()
    {
        int enemyAmount = CalculateEnemyAmount();

        for (int i = 0; i < enemyAmount; i++)
        {
            // Each spawned enemy receives its own battle difficulty and formation position.
            Enemy currentEnemy = Instantiate(enemyPrefab, enemySpawnPos, Quaternion.identity);
            currentEnemy.level = level + 3;
            currentEnemy.enemyIndex = i;
        }
    }

    // Determines how many enemies appear based on the current game level and a random chance.
    int CalculateEnemyAmount()
    {
        int enemyChance = UnityEngine.Random.Range(level, 51);

        if (enemyChance <= 25) return 1;
        if (enemyChance <= 40) return 2;
        return 3;
    }

}