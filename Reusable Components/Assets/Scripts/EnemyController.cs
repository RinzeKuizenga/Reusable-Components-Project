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

    private void Start()
    {
        enemy.onEnemyDeath += enemyDeath;
    }

    public void StartTurn()
    {
        Debug.Log($"{name} turn started");
        anchorMovement.MoveTo(anchorHolder.GetAnchor(0), 6);

        currentGrid = Instantiate(grid, transform.position, Quaternion.identity);
        currentGrid.Attack(enemyInt);
        currentGrid.onAttackDone += HandleAttackFinished;
    }

    public void MoveToIdle()
    {
        switch (enemyInt)
        {
            case 0:
                this.anchorMovement.MoveTo(anchorHolder.GetAnchor(1), 6);
                break;
            case 1:
                this.anchorMovement.MoveTo(anchorHolder.GetAnchor(2), 6);
                break;
            case 2:
                this.anchorMovement.MoveTo(anchorHolder.GetAnchor(3), 6);
                break;
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
        
    }
}
