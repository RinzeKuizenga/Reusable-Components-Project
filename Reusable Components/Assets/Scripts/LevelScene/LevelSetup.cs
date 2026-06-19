using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class LevelSetup : MonoBehaviour
{
    // Stores the current visible level and world progression values.
    public int level = 1;
    public int world = 1;

    // References used to update the level map and start the scene transition.
    [SerializeField] private List<LevelIcon> levelSprites;
    [SerializeField] private CharacterIcons characterIcons;
    [SerializeField] private GameObject transitionPrefab;


    private void Start()
    {
        // Play a transition after the character icon finishes moving.
        characterIcons.OnFinishedMoving += PlayTransition;

        // Show the correct completed levels and move the character icon to its current level.
        AssignCorrectSprites();

        // Progress to the next level after entering the level selection scene.
        StartCoroutine(NextLevelCoroutine());
    }

    // Updates all level icons and moves the character icon to the current progression level.
    void AssignCorrectSprites()
    {
        // Get the saved level instead of relying on the local LevelSetup value.
        int level = GameProgress.Instance.level;

        // Mark every previous level as completed.
        for (int i = 0; i < levelSprites.Count; i++)
        {
            levelSprites[i].SetCompleted(i < level - 1);
        }

        // Keep the target index within the available level icon range.
        int targetIndex = Mathf.Clamp(level - 1, 0, levelSprites.Count - 1);

        // Move the character icons to the current level position.
        characterIcons.MoveIcons(levelSprites[targetIndex].transform.position);
    }

    // Creates the transition effect once the character icons arrive at their target.
    void PlayTransition()
    {
        Instantiate(transitionPrefab, transform.position, Quaternion.identity);
        SFXPlayer.Instance.PlaySFX(5, 1f);
    }

    // Starts the level progression sequence after a completed battle.
    public void NextTurn()
    {
        StartCoroutine(NextLevelCoroutine());
    }

    // Waits briefly before increasing progression and updating the level map.
    IEnumerator NextLevelCoroutine()
    {
        yield return new WaitForSeconds(1f);

        // Advance the saved level while staying within the level icon limit.
        GameProgress.Instance.NextLevel(levelSprites.Count);

        // Refresh the completed icons and character position.
        AssignCorrectSprites();
    }

}