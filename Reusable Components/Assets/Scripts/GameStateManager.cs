using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.UIElements;
using System;

public enum GameState
{

    Player1Turn,

    Player2Turn,

    EnemyTurn,

    Idle
}

public class GameStateManager : MonoBehaviour
{
    public GameState CurrentState { get; private set; }
    public static GameStateManager Instance;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeState(GameState newState)
    {
        if (CurrentState == newState) return;

        CurrentState = newState;  
        Debug.Log($"GameState: {newState}");
    }

    public void SetPlayer1Turn()
    {
        ChangeState(GameState.Player1Turn);
    }

    public void SetPlayer2Turn()
    {
        ChangeState(GameState.Player2Turn);
    }

    public void SetEnemyTurn()
    {
        ChangeState(GameState.EnemyTurn);
    }

    public void SetIdleTurn()
    {
        ChangeState(GameState.Idle);
        Debug.Log($"currentState:{CurrentState}");
    }
}
