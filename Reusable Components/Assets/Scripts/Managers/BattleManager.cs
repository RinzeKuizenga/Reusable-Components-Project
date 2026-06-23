using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    // References to the two player controllers participating in the battle.
    PlayerController player1Controller;
    PlayerController player2Controller;

    // Stores all enemies currently active in this battle.
    List<EnemyController> enemies;


    // Contains every character that can take a turn, in turn order.
    List<ITurnTaker> turnOrder = new();

    // Keeps track of whose turn is currently active in the turn order list.
    public int currentTurnIndex;

    public float timeBetweenTurns;

    // Prevents battle logic from continuing after the battle has ended.
    bool battleEnded;

    public void SetupBattle()
    {
        // Find the players and enemies that were created for this battle.
        player1Controller = GameObject.FindWithTag("Player1").GetComponent<PlayerController>();
        player2Controller = GameObject.FindWithTag("Player2").GetComponent<PlayerController>();
        enemies = FindObjectsOfType<EnemyController>().ToList();

        // Listen for the moment each battle participant has finished their action.
        player1Controller.OnAttackFinished += NextTurn;
        player2Controller.OnAttackFinished += NextTurn;

        foreach (EnemyController enemy in enemies)
        {
            enemy.onAttackFinished += NextTurn;
            enemy.onEnemyDied += HandleEnemyDied;
        }

        // Build the order in which players and enemies take turns.
        BuildTurnOrder();
        Debug.Log("BattleManager finding enemies");

    }


    void BuildTurnOrder()
    {
        // Clear the previous turn order before creating a new one.
        turnOrder.Clear();

        // Players always take their turns before enemies.
        turnOrder.Add(player1Controller);
        turnOrder.Add(player2Controller);

        // Add every spawned enemy to the end of the turn order.
        foreach (EnemyController enemy in enemies)
            turnOrder.Add(enemy);

        // Log the current order for battle debugging.
        for (int i = 0; i < turnOrder.Count; i++)
        {
            Debug.Log(turnOrder[i]);
        }
    }

    public void StartCurrentTurn()
    {
        // Check whether both players are defeated before starting another turn.
        CheckForGameOver();

        // Get the current character from the turn order.
        ITurnTaker current = turnOrder[currentTurnIndex];

        // Update the broad game state so all battle actors move to the correct positions.
        if (current == player1Controller)
            GameStateManager.Instance.ChangeState(GameState.Player1Turn);
        else if (current == player2Controller)
            GameStateManager.Instance.ChangeState(GameState.Player2Turn);
        else
            GameStateManager.Instance.ChangeState(GameState.EnemyTurn);

        // Let the active player or enemy begin their own turn behaviour.
        current.StartTurn();

        Debug.Log(GameStateManager.Instance.CurrentState);
    }

    public void NextTurn()
    {
        // Wait briefly before moving to the next turn for clearer battle pacing.
        StartCoroutine(NextTurnCoroutine(timeBetweenTurns));
    }

    IEnumerator NextTurnCoroutine(float duration)
    {
        // Wait before switching to the next battle participant.
        yield return new WaitForSeconds(duration);

        // Continue moving through the turn order until a valid participant is found.
        do
        {
            currentTurnIndex++;

            // Return to the beginning after reaching the end of the list.
            if (currentTurnIndex >= turnOrder.Count)
                currentTurnIndex = 0;

        } while (ShouldSkipTurn(turnOrder[currentTurnIndex]));

        StartCurrentTurn();
    }

    void HandleEnemyDied(EnemyController enemy)
    {
        // Ignore additional death events after the battle has already ended.
        if (battleEnded) return;

        // Remove the defeated enemy from battle tracking and turn order.
        enemies.Remove(enemy);
        turnOrder.Remove(enemy);

        // The battle is won when no enemies remain.
        if (enemies.Count == 0)
        {
            Debug.Log("LOAD SCENE");
            battleEnded = true;
            SceneLoader.Instance.LoadScene("LevelBar");
            return;
        }

        // Keep the turn index within the valid range after removing an enemy.
        if (currentTurnIndex >= turnOrder.Count)
            currentTurnIndex = 0;
    }

    bool ShouldSkipTurn(ITurnTaker current)
    {
        // Skip defeated players.
        if (current == player1Controller && player1Controller.isDead)
            return true;

        if (current == player2Controller && player2Controller.isDead)
            return true;

        // Skip enemies that have been destroyed during battle.
        if (current is EnemyController enemy && enemy == null)
            return true;

        return false;
    }

    void CheckForGameOver()
    {
        // End the battle when both players have been defeated.
        if (player1Controller.isDead && player2Controller.isDead)
        {
            GameOverManager.Instance.GameOver();
            PlaylistPlayer.Instance.FadeOut();
            GameProgress.Instance.ResetProgress();
            return;
        }
    }

}