using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class EnemyController : MonoBehaviour
{
    AnchorHolder anchorHolder;
    AnchorMovement anchorMovement;
    Enemy enemy;

    public int enemyInt;

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

                switch (enemyInt)
                {
                    case 0:
                        anchorMovement.MoveTo(anchorHolder.GetAnchor(1), 6);
                        break;
                    case 1:
                        anchorMovement.MoveTo(anchorHolder.GetAnchor(2), 6);
                        break;
                    case 2:
                        anchorMovement.MoveTo(anchorHolder.GetAnchor(3), 6);
                        break;
                }
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
