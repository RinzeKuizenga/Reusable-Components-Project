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
    Player2Target,

    EnemyTurn
}

public class GameStateManager : MonoBehaviour
{
    public GameState CurrentState { get; private set; }
    public static GameStateManager Instance;
    public event Action<GameState> onStateChanged;

    [SerializeField] private MenuController menuController;
    [SerializeField] private MenuController menuPrefab;

    [SerializeField] private Transform player1Transform;

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

        if(newState == GameState.Player1Turn)
        {
             menuPrefab = Instantiate(menuController, player1Transform.position + Vector3.up * 2, Quaternion.identity);
        }
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
    public void SetPlayer2Action()
    {
        ChangeState(GameState.Player2Action);
    }
    public void SetPlayer2Target()
    {
        ChangeState(GameState.Player2Target);
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
