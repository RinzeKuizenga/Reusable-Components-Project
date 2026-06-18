using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System;

public class Player : MonoBehaviour, IDamagable, IHealable
{
    [SerializeField] private DamageFeedback damagePrefab;
    [SerializeField] private DamageFeedback healthPrefab;
    [SerializeField] private GameObject cloudPrefab;
    [SerializeField] private GameObject healPrefab;
    public int health { get; set; }

    public int stamina;

    public int maxHealth;
    public int MaxHealth => maxHealth;

    public Action OnDamageTaken;
    public Action OnHealed;
    //public Action OnStamindaUsed;
    public Action OnPlayerDeath;

    public int level;

    private void Awake()
    {
        health = maxHealth;
    }
    public void Damage(int damage)
    {
        if (health <= 0) return;
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);

        DamageFeedback damageFeedback = Instantiate(damagePrefab, transform.position + Vector3.up, Quaternion.identity);
        damageFeedback.DisplayDamage(damage);

        Instantiate(cloudPrefab, transform.position, Quaternion.identity);

        OnDamageTaken?.Invoke();
        SFXPlayer.Instance.PlaySFX(6, 1);

        if (health <= 0) OnPlayerDeath?.Invoke();
    }

    public void Heal(int points)
    {
        if (health <= 0) return;
        health += points;
        health = Mathf.Clamp(health, 0, maxHealth);

        DamageFeedback damageFeedback = Instantiate(healthPrefab, transform.position + Vector3.up, Quaternion.identity);
        damageFeedback.DisplayDamage(points);

        Instantiate(healPrefab, transform.position, Quaternion.identity);

        OnHealed?.Invoke();
        SFXPlayer.Instance.PlaySFX(7, 1);
    }
}
