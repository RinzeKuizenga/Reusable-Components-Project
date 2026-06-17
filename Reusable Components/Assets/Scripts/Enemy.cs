using UnityEngine;
using System;

public class Enemy : MonoBehaviour, IDamagable
{
    public int enemyIndex = 0;

    [SerializeField] private EnemyData enemyData;

    [SerializeField] private DamageFeedback damagePrefab;
    public int health { get; set; }
    int maxHealth;
    public int level;

    SpriteRenderer spriteRenderer;

    public Action onEnemyDeath;



    void Awake()
    {
        if (enemyData != null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            maxHealth = enemyData.maxhealth;
            spriteRenderer.sprite = enemyData.sprite;
        }
    }

    public void Start()
    {
        CalculateHealth();
        health = maxHealth;
    }

    void CalculateHealth()
    {
        maxHealth = Mathf.RoundToInt(maxHealth + level * 1.5f + 1);
    }

    public void Damage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);

        DamageFeedback damageFeedback = Instantiate(damagePrefab, transform.position + Vector3.up, Quaternion.identity);
        damageFeedback.DisplayDamage(damage);
        if (health <= 0) onEnemyDeath?.Invoke();
        Debug.Log($"{health}");
    }
}
