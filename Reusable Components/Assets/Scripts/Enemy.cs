using UnityEngine;
using System;

public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] private DamageFeedback damagePrefab;
    public int health { get; set; }

    [SerializeField] private int maxHealth;

    public Action onEnemyDeath;

    public void Start()
    {
        health = maxHealth;
    }

    public void Damage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);

        DamageFeedback damageFeedback = Instantiate(damagePrefab, transform.position + Vector3.up, Quaternion.identity);
        damageFeedback.DisplayDamage(damage);
        onEnemyDeath?.Invoke();
        Debug.Log($"{health}");
    }
}
