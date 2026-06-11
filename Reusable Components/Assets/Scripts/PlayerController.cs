using UnityEngine;
using System;
using UnityEngine.InputSystem.XR.Haptics;

public class PlayerController : MonoBehaviour, ITurnTaker
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
    }
    public void StartTurn()
    {
        Debug.Log($"{name} turn started");
        anchorMovement.MoveTo(anchorHolder.GetAnchor(0), 16);
    }

    public void MoveToIdle()
    {
        anchorMovement.MoveTo(anchorHolder.GetAnchor(2), 16);
    }

    void FixedUpdate()
    {
        if (GameStateManager.Instance.CurrentState == GameState.EnemyTurn && !anchorMovement.isMoving && !isDead)
        {
            freeMovement.Move(_input.GetInput());
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
        command.enemy.Damage(command.attack.damage);
        OnAttackFinished?.Invoke();
    }

    void HandleDeath()
    {
        isDead = true;
    }

    private void OnDestroy()
    {
    }
}
