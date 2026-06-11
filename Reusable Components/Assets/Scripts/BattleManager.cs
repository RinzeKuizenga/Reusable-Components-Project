using Unity.Mathematics;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BattleManager : MonoBehaviour
{
    PlayerController player1Controller;
    PlayerController player2Controller;
    List<EnemyController> enemies;

    List<ITurnTaker> turnOrder = new();
    public int currentTurnIndex;

    public void Awake()
    {
        player1Controller = GameObject.FindWithTag("Player1").GetComponent<PlayerController>();
        player2Controller = GameObject.FindWithTag("Player2").GetComponent<PlayerController>();
        
        enemies = new List<EnemyController>(FindObjectsByType<EnemyController>());

        player1Controller.OnAttackFinished += NextState;
        player2Controller.OnAttackFinished += NextState;
        foreach(EnemyController enemy in enemies)
        {
            enemy.onAttackFinished += NextState;
        }
    }

    private void Start()
    {
        BuildTurnOrder() ;
        StartGame();
    }

    public void NextState()
    {
        Debug.Log($"CurrentState: {GameStateManager.Instance.CurrentState})");

        StartCoroutine(NextTurnCoroutine());


        Debug.Log($"Going to State: {GameStateManager.Instance.CurrentState})");
    }

    IEnumerator NextTurnCoroutine()
    {
        for (int i = 0; i < turnOrder.Count; i++) turnOrder[i].MoveToIdle();
            
        yield return new WaitForSeconds(1f);

        currentTurnIndex++;

        if(currentTurnIndex >= turnOrder.Count)
        {
            BuildTurnOrder();
            currentTurnIndex = 0;
        }

        turnOrder[currentTurnIndex].StartTurn();
        Debug.Log(currentTurnIndex);
    }

    void BuildTurnOrder()
    {
        turnOrder.Clear();
        int index = -1;
        turnOrder.Add(player1Controller);
        turnOrder.Add(player2Controller);
        if (enemies != null)
        {
            foreach (EnemyController enemy in enemies)
            {
                turnOrder.Add(enemy);
            }
        }
        for (int i = 0; i < turnOrder.Count; i++)
        {
            Debug.Log($"[{i}] {turnOrder[i]}");
        }

    }
    void StartGame()
    {
        Debug.Log("Starting first turn");
        turnOrder[currentTurnIndex].StartTurn();
    }
}
