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

    public Action OnAttackFinished;
    public Action OnPlayerDeath;

    public bool isDead = false;
    bool isMyTurn;

    [SerializeField] private ControlHint controlHintPrefab;

    private bool hasShownControlHint;

    void Awake()
    {
        anchorHolder = GetComponent<AnchorHolder>();
        anchorMovement = GetComponent<AnchorMovement>();
        player = GetComponent<Player>();

        _input = GetComponent<IInput>();
    }

    void Start()
    {
        GameStateManager.Instance.onStateChanged += HandleStateChanged; 

        anchorMovement.onFinishedMoving += HandleFinishMoving;
        player.OnPlayerDeath += HandleDeath;

    }

    void FixedUpdate()
    {
        if (GameStateManager.Instance.CurrentState != GameState.EnemyTurn ||
            isDead ||
            freeMovement == null ||
            anchorMovement.isMoving)
        {
            return;
        }

        freeMovement.Move(_input.GetInput());
    }

    public void StartTurn()
    {
        isMyTurn = true;
    }
    void HandleStateChanged(GameState newState)
    {
        // Enemy starts attacking:
        // add FreeMovement so players can dodge/run around.
        if (newState == GameState.EnemyTurn && !isDead)
        {
            AddFreeMovement();
        }
        // Player turn or idle:
        // remove FreeMovement so the player cannot walk during the menu.
        else
        {
            RemoveFreeMovement();
        }

        if (isDead) return;

        if (newState != GameState.EnemyTurn)
        {
            hasShownControlHint = false;
        }

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
        if (!isMyTurn) return;
        
            MenuController menu = Instantiate(menuPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
            menu.WhichPlayer(this);
            menu.onAttackChosen += HandleAttackChosen;
            menu.onItemChosen += HandleItemChosen;
        
    }

    void HandleAttackChosen(AttackCommand command)
    {
        Debug.Log("HANDLE ATTACK");
        command.enemy.Damage(command.attack.damage);
        isMyTurn = false;
        OnAttackFinished?.Invoke();
    }

    void HandleItemChosen(ItemCommand command)
    {
        Debug.Log("HANDLE ITEM");
        command.target.Heal(command.item.effective);
        isMyTurn = false;
        OnAttackFinished?.Invoke();
    }

    void HandleDeath()
    {
        isDead = true;
        RemoveFreeMovement();
    }

    void ShowControlHint()
    {
        hasShownControlHint = true;

        ControlHint hint = Instantiate(controlHintPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
        hint.Setup(transform);
    }

    void AddFreeMovement()
    {
        if (freeMovement != null) return;

        freeMovement = gameObject.AddComponent<FreeMovement>();

        Debug.Log("FreeMovement toegevoegd: speler kan nu bewegen");
    }

    void RemoveFreeMovement()
    {
        if (freeMovement == null) return;

        freeMovement.StopMove();
        Destroy(freeMovement);
        freeMovement = null;

        Debug.Log("FreeMovemeent verwijderd: speler kan niet meer bewegen");
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.onStateChanged -= HandleStateChanged;
    }
}
