using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.UIElements;
using System;

public enum GameState
{
    Idle,

    Player1Turn,
    Player1Action,
    Player1Target,

    Player2Turn,
    Player2Action,
    Player3Target,

    EnemyTurn
}

public class GameStateManager : MonoBehaviour
{
    public GameState CurrentState { get; private set; }
    public static GameStateManager Instance;
    public event Action<GameState> onStateChanged;

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

    private void Start()
    {
        SetPlayer1Turn();
    }

    public void ChangeState(GameState newState)
    {
        if (CurrentState == newState) return;

        CurrentState = newState;
        onStateChanged?.Invoke(CurrentState);   
        Debug.Log($"GameState: {newState}");
    }

    public void SetPlayer1Turn()
    {
        ChangeState(GameState.Player1Turn);
    }
    public void SetPlayer1Action()
    {
        ChangeState(GameState.Player1Action);
    }
    public void SetPlayer1Target()
    {
        ChangeState(GameState.Player1Target);
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
    }
}
