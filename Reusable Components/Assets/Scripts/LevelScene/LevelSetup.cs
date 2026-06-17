using UnityEngine;
using System.Collections.Generic;

public class LevelSetup : MonoBehaviour
{

    public int level = 1;
    public int world = 1;

    [SerializeField] private List<LevelIcon> levelSprites;
    [SerializeField] private CharacterIcons characterIcons;


    private void Start()
    {
        Debug.Log(level);
        AssignCorrectSprites();
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

    public void NextLevel()
    {
        level++;
        if (level > levelSprites.Count)
        {
            LevelIcon lastLevelIcon = levelSprites[levelSprites.Count - 1];
            lastLevelIcon.StopAnimator();

            world++;
        }
        AssignCorrectSprites();

    }
}
