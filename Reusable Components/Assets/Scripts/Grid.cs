using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.Tilemaps;

public class Grid : MonoBehaviour
{
    public Action onAttackDone;

    List<GridTile> dangerousTiles = new();
    [SerializeField] private List<GridTile> gridTiles;

    public void Attack(int enemylevel)
    {
        StartCoroutine(AttackCoroutine(enemylevel));
    }

    void ChooseGrids()
    {
        dangerousTiles.Clear();
        foreach (GridTile tile in gridTiles)
        {
            tile.SetDangerous(false);
        }
        int dangerousGrid = UnityEngine.Random.Range(4, 8);

        for (int i = 0; i < dangerousGrid; i++)
        {
            GridTile randomTile = gridTiles[UnityEngine.Random.Range(0, gridTiles.Count)];
            while (dangerousTiles.Contains(randomTile))
            {
                randomTile = gridTiles[UnityEngine.Random.Range(0, gridTiles.Count)];
            }
            dangerousTiles.Add(randomTile);
            randomTile.SetIdle();
        }
    }

    IEnumerator AttackCoroutine(int enemyLevel)
    {
        float delay = Mathf.Max(1.5f, 4f - enemyLevel * 0.15f);
        int attackAmount = UnityEngine.Random.Range(1, 4);

        for (int i = 0; i < attackAmount; i++)
        {
            ChooseGrids();
            yield return new WaitForSeconds(delay - 0.1f);

            ShowDangerousTiles();

            yield return new WaitForSeconds(0.1f);
        }

        onAttackDone?.Invoke();
        Destroy(gameObject);
    }

    void ShowDangerousTiles()
    {
        foreach(GridTile tile in dangerousTiles)
        {
            tile.SetDangerous(true);
            tile.DamageAnythingInside();
        }
    }
}
