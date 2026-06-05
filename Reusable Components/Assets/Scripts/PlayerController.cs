using UnityEngine;
using System;
using UnityEngine.InputSystem.XR.Haptics;

public class PlayerController : MonoBehaviour
{
    FreeMovement freeMovement;
    AnchorHolder anchorHolder;
    AnchorMovement anchorMovement;
    Player player;

    IInput _input;

    [SerializeField] private MenuController menuPrefab;
    [SerializeField] private GameState goodState;

    public Action OnAttackFinished;
    public Action OnPlayerDeath;

    bool isDead = false;

    void Awake()
    {
        freeMovement = GetComponent<FreeMovement>();
        anchorHolder = GetComponent<AnchorHolder>();
        anchorMovement = GetComponent<AnchorMovement>();
        player = GetComponent<Player>();

        _input = GetComponent<IInput>();
    }

    private void Start()
    {
        anchorMovement.onFinishedMoving += HandleFinishMoving;
        player.OnPlayerDeath += HandleDeath;
        GameStateManager.Instance.onStateChanged += HandleStateChanged;
    }
    void FixedUpdate()
    {
        if (GameStateManager.Instance.CurrentState == GameState.EnemyTurn && !anchorMovement.isMoving && !isDead)
        {
            freeMovement.Move(_input.GetInput());
        }
    }

    void HandleStateChanged(GameState newState)
    {
        if (isDead) return;
        freeMovement.StopMove();
        switch (newState)
        {
            case GameState.Player1Turn:
                anchorMovement.MoveTo(anchorHolder.GetAnchor(0), 16);
                break;

            case GameState.Player2Turn:
                anchorMovement.MoveTo(anchorHolder.GetAnchor(1), 16);
                break;

            case GameState.Idle:
                anchorMovement.MoveTo(anchorHolder.GetAnchor(2), 16);
                break;
            case GameState.EnemyTurn:
                anchorMovement.MoveTo(anchorHolder.GetAnchor(3), 6);
                break;
        }
    }

    void HandleFinishMoving()
    {
        if (GameStateManager.Instance.CurrentState == goodState)
        {
            MenuController menu = Instantiate(menuPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
            menu.WhichPlayer(this);
            menu.onAttackChosen += HandleAttackChosen;
        }
    }

    void HandleAttackChosen(AttackCommand command)
    {
        Debug.Log("HANDLE ATTACK");
        command.target.Damage(command.attack.damage);
        OnAttackFinished?.Invoke();
    }

    void HandleDeath()
    {
        isDead = true;
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.onStateChanged -= HandleStateChanged;
    }
}
