using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using Mono.Cecil;

public class Grid : MonoBehaviour
{
    [SerializeField] private List<GameObject> gridTiles;

    public Sprite safeSprite;
    public Sprite dangerSprite;
    public bool isDangerous;
    public Action onAttackDone;

  IDamagable _damagable;
    List<int> dangerousTiles = new();

    private void Awake()
    {
        _damagable = GetComponent<IDamagable>();

    }

    public void Attack(int enemylevel)
    {
        StartCoroutine(AttackCoroutine(enemylevel));
    }

    void ChooseGrids()
    {
        dangerousTiles.Clear();
        for (int j = 0; j < gridTiles.Count; j++)
        {
            SpriteRenderer spriteRenderer = gridTiles[j].GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = safeSprite;
        }
        int dangerousGrid = UnityEngine.Random.Range(4, 8);

        for (int i = 0; i < dangerousGrid; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, gridTiles.Count);
            while (dangerousTiles.Contains(randomIndex))
            {
                randomIndex = UnityEngine.Random.Range(0, gridTiles.Count);
            }
            SpriteRenderer spriteRenderer = gridTiles[randomIndex].GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = dangerSprite;

            dangerousTiles.Add(randomIndex);
        }
    }

    IEnumerator AttackCoroutine(int enemyLevel)
    {
        float delay = Mathf.Max(1.5f, 4f - enemyLevel * 0.15f);
        int attackAmount = UnityEngine.Random.Range(1, 4);

        for (int i = 0; i < attackAmount; i++)
        {
            ChooseGrids();
            yield return new WaitForSeconds(delay);
        }

        onAttackDone?.Invoke();
        Destroy(gameObject);
    }
}
