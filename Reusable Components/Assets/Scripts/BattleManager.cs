using Unity.Mathematics;
using UnityEngine;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    PlayerController player1Controller;
    PlayerController player2Controller;
    List<EnemyController> enemies;


    public int currentTurnIndex;

    public void Awake()
    {
        player1Controller = GameObject.FindWithTag("Player1").GetComponent<PlayerController>(); 
        player2Controller = GameObject.FindWithTag("Player2").GetComponent<PlayerController>();

        player1Controller.OnAttackFinished += NextState;
        player2Controller.OnAttackFinished += NextState;
    }

   public void NextState()
    {
        Debug.Log($"CurrentState: {GameStateManager.Instance.CurrentState})");

        currentTurnIndex = (int)GameStateManager.Instance.CurrentState;
        GameStateManager.Instance.ChangeState((GameState)currentTurnIndex + 1);

        if (currentTurnIndex > 4) currentTurnIndex = 0;


        Debug.Log($"Going to State: {GameStateManager.Instance.CurrentState})");
    }
}
