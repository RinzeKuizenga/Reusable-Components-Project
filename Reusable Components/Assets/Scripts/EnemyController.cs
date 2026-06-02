using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class EnemyController : MonoBehaviour
{
    AnchorHolder anchorHolder;
    AnchorMovement anchorMovement;
    Enemy enemy;

    void Awake()
    {
        anchorHolder = GetComponent<AnchorHolder>();
        anchorMovement = GetComponent<AnchorMovement>();
        enemy = GetComponent<Enemy>();  
    }

    private void Start()
    {
        GameStateManager.Instance.onStateChanged += HandleStateChanged;
        enemy.onEnemyDeath += enemyDeath;
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

    void enemyDeath()
    {

    }

    private void OnDestroy()
    {
        GameStateManager.Instance.onStateChanged -= HandleStateChanged;
    }
}
