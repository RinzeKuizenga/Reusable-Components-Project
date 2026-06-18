using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class LevelSetup : MonoBehaviour
{

    public int level = 1;
    public int world = 1;

    [SerializeField] private List<LevelIcon> levelSprites;
    [SerializeField] private CharacterIcons characterIcons;
    [SerializeField] private GameObject transitionPrefab;


    private void Start()
    {
        characterIcons.OnFinishedMoving += PlayTransition;
        AssignCorrectSprites();
        StartCoroutine(NextLevelCoroutine());
    }

    void AssignCorrectSprites()
    {
        int level = GameProgress.Instance.level;

        for (int i = 0; i < levelSprites.Count; i++)
        {
            levelSprites[i].SetCompleted(i < level - 1);
        }

        int targetIndex = Mathf.Clamp(level - 1, 0, levelSprites.Count - 1);
        characterIcons.MoveIcons(levelSprites[targetIndex].transform.position);
    }

    void PlayTransition()
    {
        Instantiate(transitionPrefab, transform.position, Quaternion.identity);
        SFXPlayer.Instance.PlaySFX(5, 1f);
    }

    IEnumerator NextLevelCoroutine()
    {
        yield return new WaitForSeconds(1f);
        GameProgress.Instance.NextLevel();
        AssignCorrectSprites(); 
    }

}
