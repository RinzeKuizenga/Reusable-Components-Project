using UnityEngine;
using System;
using UnityEngine.InputSystem.XR.Haptics;

public class EnemyController : MonoBehaviour, ITurnTaker
{
    // Cached components used for enemy movement, formation positions, and enemy stats.
    AnchorHolder anchorHolder;
    AnchorMovement anchorMovement;
    Enemy enemy;

    // Grid prefab used for this enemy's attack pattern.
    [SerializeField] private Grid grid;

    // Stores the grid instance created during the current enemy turn.
    Grid currentGrid;

    // Notifies the BattleManager when this enemy has completed its attack.
    public Action onAttackFinished;

    // Sends this enemy reference when it is defeated so it can be removed from battle tracking.
    public Action<EnemyController> onEnemyDied;

    // Stores this enemy's position in the enemy battle formation.
    public int enemyInt;

    void Awake()
    {
        // Cache required components attached to this enemy GameObject.
        anchorHolder = GetComponent<AnchorHolder>();
        anchorMovement = GetComponent<AnchorMovement>();
        enemy = GetComponent<Enemy>();
    }

    void Start()
    {
        // Listen for broad battle state changes to move back into formation.
        GameStateManager.Instance.onStateChanged += HandleStateChanged;

        // React when the Enemy component reaches zero health.
        enemy.onEnemyDeath += enemyDeath;

        // Store the formation index assigned when this enemy was spawned.
        enemyInt = enemy.enemyIndex;
    }

    // Starts this enemy's turn by moving forward and creating its grid attack.
    public void StartTurn()
    {
        // Move this enemy to the front attack position.
        anchorMovement.MoveTo(anchorHolder.GetAnchor(0), 6);

        // Create a new grid instance and start its attack sequence.
        currentGrid = Instantiate(grid, transform.position, Quaternion.identity);
        currentGrid.Attack(enemy.level);

        // Wait for the grid to finish before ending this enemy's turn.
        currentGrid.onAttackDone += HandleAttackFinished;
    }

    // Moves this enemy back into its formation position after the enemy turn ends.
    void HandleStateChanged(GameState newState)
    {
        // Keep the enemy at the front while enemies are actively taking turns.
        if (newState != GameState.EnemyTurn)
        {
            // Each enemy uses a different anchor based on its formation index.
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

    // Ends the enemy turn after the grid attack sequence has completed.
    void HandleAttackFinished()
    {
        Debug.Log("Grid Finished"); ;

        // Tell the BattleManager that the next participant can take a turn.
        onAttackFinished?.Invoke();

        // Return this enemy to its assigned formation position.
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

    // Notifies battle systems that this enemy died before removing the GameObject.
    void enemyDeath()
    {
        onEnemyDied?.Invoke(this);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        // Remove the state listener when this enemy is destroyed.
        GameStateManager.Instance.onStateChanged -= HandleStateChanged;
    }
}