using UnityEngine;
using System;
using UnityEngine.InputSystem.XR.Haptics;

public class EnemyController : MonoBehaviour, ITurnTaker
{
    AnchorHolder anchorHolder;
    AnchorMovement anchorMovement;
    Enemy enemy;

    [SerializeField] private Grid grid;
    Grid currentGrid;
    Animator gridAnimator;
    public Action onAttackFinished;

    public int enemyInt;

    void Awake()
    {
        anchorHolder = GetComponent<AnchorHolder>();
        anchorMovement = GetComponent<AnchorMovement>();
        enemy = GetComponent<Enemy>();
        gridAnimator = grid.GetComponent<Animator>();
    }

    void Start()
    {
        GameStateManager.Instance.onStateChanged += HandleStateChanged;
        enemy.onEnemyDeath += enemyDeath;
    }
    public void StartTurn()
    {
        anchorMovement.MoveTo(anchorHolder.GetAnchor(0), 6);

        currentGrid = Instantiate(grid, transform.position, Quaternion.identity);
        currentGrid.Attack(enemyInt);
        currentGrid.onAttackDone += HandleAttackFinished;
    }

    void HandleStateChanged(GameState newState)
    {

        if (newState != GameState.EnemyTurn)
        {
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
        }
    }


        void HandleAttackFinished()
    {
        Debug.Log("Grid Finished");;
        onAttackFinished?.Invoke();
    }

    void enemyDeath()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.onStateChanged -= HandleStateChanged;
    }
}
