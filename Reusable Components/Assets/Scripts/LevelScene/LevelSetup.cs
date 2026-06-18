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
        NextLevel();
    }

    void AssignCorrectSprites()
    {
        for (int i = 0; i < level -1; i++)
        {
            Debug.Log(level);
            Debug.Log(levelSprites.Count);
            levelSprites[i].SetCompleted(true);
            //if (level  > levelSprites.Count) return;
            characterIcons.MoveIcons(levelSprites[i + 1].transform.position);       
        }
    }

    void NextLevel()
    {
        StartCoroutine(NextLevelCoroutine());
    }
    

    IEnumerator NextLevelCoroutine()
    {
        yield return new WaitForSeconds(1f);
        level++;
        if (level > levelSprites.Count)
        {
            LevelIcon lastLevelIcon = levelSprites[levelSprites.Count - 1];
            lastLevelIcon.StopAnimator();

            world++;
        }
        AssignCorrectSprites();
    }

    void PlayTransition()
    {
        Instantiate(transitionPrefab, transform.position, Quaternion.identity);
        SFXPlayer.Instance.PlaySFX(5, 1f);
    }
}
