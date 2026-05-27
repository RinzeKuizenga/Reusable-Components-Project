using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class EnemyController : MonoBehaviour
{
    KeyboardInput keyboardInput;
    FreeMovement freeMovement;
    AnchorHolder anchorHolder;
    AnchorMovement anchorMovement;

    void Awake()
    {
        anchorHolder = GetComponent<AnchorHolder>();
        anchorMovement = GetComponent<AnchorMovement>();
    }

    private void Start()
    {
        GameStateManager.Instance.onStateChanged += HandleStateChanged;
    }

    void HandleStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.EnemyTurn:
                anchorMovement.MoveTo(anchorHolder.GetAnchor(0), 6);
                break;
            default:
                anchorMovement.MoveTo(anchorHolder.GetAnchor(1), 16);
                break;
        }
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.onStateChanged -= HandleStateChanged;
    }
}
