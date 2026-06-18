using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class BattleManager : MonoBehaviour
{
    PlayerController player1Controller;
    PlayerController player2Controller;
    List<EnemyController> enemies;


    List<ITurnTaker> turnOrder = new();
    public int currentTurnIndex;
    public float waitTime;

    bool battleEnded;

    public void SetupBattle()
    {
        player1Controller = GameObject.FindWithTag("Player1").GetComponent<PlayerController>();
        player2Controller = GameObject.FindWithTag("Player2").GetComponent<PlayerController>();
        enemies = FindObjectsOfType<EnemyController>().ToList();

        player1Controller.OnAttackFinished += NextTurn;
        player2Controller.OnAttackFinished += NextTurn;
        foreach (EnemyController enemy in enemies)
        {
            enemy.onAttackFinished += NextTurn;
            enemy.onEnemyDied += HandleEnemyDied;
        }
        BuildTurnOrder();
        Debug.Log("BattleManager finding enemies");

    }


    void BuildTurnOrder()
    {
        turnOrder.Clear();

        turnOrder.Add(player1Controller);
        turnOrder.Add(player2Controller);

        foreach (EnemyController enemy in enemies)
            turnOrder.Add(enemy);

        for (int i = 0; i < turnOrder.Count; i++)
        {
            Debug.Log(turnOrder[i]);
        }
    }

    public void StartCurrentTurn()
    {
        CheckForGameOver();

        ITurnTaker current = turnOrder[currentTurnIndex];

        if (current == player1Controller)
            GameStateManager.Instance.ChangeState(GameState.Player1Turn);
        else if (current == player2Controller)
            GameStateManager.Instance.ChangeState(GameState.Player2Turn);
        else
            GameStateManager.Instance.ChangeState(GameState.EnemyTurn);

        current.StartTurn();

        Debug.Log(GameStateManager.Instance.CurrentState);
    }

    public void NextTurn()
    {
        StartCoroutine(NextTurnCoroutine(0.5f));
    }

    IEnumerator NextTurnCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        do
        {
            currentTurnIndex++;

            if (currentTurnIndex >= turnOrder.Count)
                currentTurnIndex = 0;

        } while (ShouldSkipTurn(turnOrder[currentTurnIndex]));

        StartCurrentTurn();
    }

    bool ShouldSkipTurn(ITurnTaker current)
    {
        if (current == player1Controller && player1Controller.isDead)
            return true;

        if (current == player2Controller && player2Controller.isDead)
            return true;

        if (current is EnemyController enemy && enemy == null)
            return true;

        return false;
    }

    void CheckForGameOver()
    {
        if (player1Controller.isDead && player2Controller.isDead)
        {
            GameOverManager.Instance.GameOver();
            PlaylistPlayer.Instance.FadeOut();
            GameProgress.Instance.ResetProgress();
            return;
        }
    }
    void HandleEnemyDied(EnemyController enemy)
    {
        if (battleEnded) return;
        enemies.Remove(enemy);
        turnOrder.Remove(enemy);

        if (enemies.Count == 0)
        {
            Debug.Log("LOAD SCENE");
            battleEnded = true;
            SceneLoader.Instance.LoadScene("LevelBar");
            return;
        }

        if (currentTurnIndex >= turnOrder.Count)
            currentTurnIndex = 0;
    }

}
