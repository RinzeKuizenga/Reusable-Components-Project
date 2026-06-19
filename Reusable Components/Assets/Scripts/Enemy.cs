using UnityEngine;
using System;

public class Enemy : MonoBehaviour, IDamagable
{
    // Used to determine this enemy's position in the battle formation.
    public int enemyIndex = 0;

    // Stores the base stats and visual data for this enemy type.
    [SerializeField] private EnemyData enemyData;

    // Prefabs used to show feedback when the enemy is damaged.
    [SerializeField] private DamageFeedback damagePrefab;
    [SerializeField] private GameObject cloudPrefab;

    // Current health is changed through the Damage method.
    public int health { get; set; }

    // Base maximum health, increased depending on the enemy level.
    int maxHealth;

    // Used to scale this enemy's health and difficulty.
    public int level;

    // Cached reference used to assign the sprite from EnemyData.
    SpriteRenderer spriteRenderer;

    // Lets other systems react when this enemy is defeated.
    public Action onEnemyDeath;



    void Awake()
    {
        // Load visual and health values from the assigned EnemyData.
        if (enemyData != null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            maxHealth = enemyData.maxhealth;
            spriteRenderer.sprite = enemyData.sprite;
        }
    }

    public void Start()
    {
        // Calculate the final health after the level has been assigned at runtime.
        CalculateHealth();
        health = maxHealth;
    }

    void CalculateHealth()
    {
        // Scale enemy health upward as the game level increases.
        maxHealth = Mathf.RoundToInt(maxHealth + level * 1.5f + 1);
    }

    // Reduces health, creates feedback effects, and notifies listeners on death.
    public void Damage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);

        // Spawn a floating damage number above the enemy.
        DamageFeedback damageFeedback = Instantiate(damagePrefab, transform.position + Vector3.up, Quaternion.identity);
        damageFeedback.DisplayDamage(damage);

        Instantiate(cloudPrefab, transform.position, Quaternion.identity);

        // Notify battle systems when this enemy has been defeated.
        if (health <= 0) onEnemyDeath?.Invoke();

        SFXPlayer.Instance.PlaySFX(6, 1);
        Debug.Log($"{health}");
    }
}