using Unity.Mathematics;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class BattleManager : MonoBehaviour
{
    PlayerController player1Controller;
    PlayerController player2Controller;
    List<EnemyController> enemies;


    List<ITurnTaker> turnOrder = new();
    public int currentTurnIndex;

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
        if (player1Controller.isDead && player2Controller.isDead) GameOverManager.Instance.GameOver(); 
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
}
