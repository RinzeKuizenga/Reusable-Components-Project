using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class PlayerController : MonoBehaviour
{
    FreeMovement freeMovement;
    AnchorHolder anchorHolder;
    AnchorMovement anchorMovement;

    IInput _input;

    void Awake()
    {
        freeMovement = GetComponent<FreeMovement>();
        anchorHolder = GetComponent<AnchorHolder>();    
        anchorMovement = GetComponent<AnchorMovement>();

        _input = GetComponent<IInput>();
    }

    private void Start()
    {
        GameStateManager.Instance.onStateChanged += HandleStateChanged;
    }
    void Update()
    {
        if (GameStateManager.Instance.CurrentState == GameState.EnemyTurn && !anchorMovement.isMoving)
        {
            freeMovement.Move(_input.GetInput());
        }
    }

    void HandleStateChanged(GameState newState)
    {
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

    private void OnDestroy()
    {
        GameStateManager.Instance.onStateChanged -= HandleStateChanged;
    }
}
