using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System;

public class Player : MonoBehaviour, IDamagable
{
    [SerializeField] private DamageFeedback damagePrefab;
    public int health { get; set; }

    public int stamina;

    public int maxHealth;

    public Action OnDamageTaken;

    private void Awake()
    {
        health = maxHealth;
    }
    public void Damage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        Debug.Log($"{health}");

        DamageFeedback damageFeedback = Instantiate(damagePrefab, transform.position + Vector3.up, Quaternion.identity);
        damageFeedback.DisplayDamage(damage);

        OnDamageTaken?.Invoke();
    }
}
