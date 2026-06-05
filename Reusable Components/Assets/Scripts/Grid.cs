using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class Grid : MonoBehaviour
{
    [SerializeField] private List<GameObject> gridTiles;

    public Sprite safeSprite;
    public Sprite dangerSprite;

    private void Awake()
    {
        //gridTiles.ForEach(GetComponent<SpriteRenderer>());
        //gridTiles.ForEach(GetComponent<BoxCollider2D>());
    }

    void ChooseGrids()
    {
        int index = Random.Range(0, gridTiles.Count);
        for (int i = 0; i < index; i++)
        {

        }
    }
}
