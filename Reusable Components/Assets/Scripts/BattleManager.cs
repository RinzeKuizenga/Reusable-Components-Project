using Unity.Mathematics;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    PlayerController player1Controller;
    PlayerController player2Controller;
    Grid grid;

    public void Awake()
    {
        player1Controller = GameObject.FindWithTag("Player1").GetComponent<PlayerController>(); 
        player2Controller = GameObject.FindWithTag("Player2").GetComponent<PlayerController>();

        player1Controller.OnAttackFinished += NextState;
        player2Controller.OnAttackFinished += NextState;
    }

   public void NextState()
    {
        if (grid != null)
        {
            grid = GameObject.FindWithTag("Grid").GetComponent<Grid>();
            grid.onAttackDone += NextState;
        }

        Debug.Log($"CurrentState: {GameStateManager.Instance.CurrentState})");
        int index = (int)GameStateManager.Instance.CurrentState;
        GameStateManager.Instance.ChangeState((GameState)index + 1);
        Debug.Log($"States:{index})");
        Debug.Log($"Going to State: {GameStateManager.Instance.CurrentState})");
    }
}
