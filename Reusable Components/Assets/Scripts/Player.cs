
using UnityEngine;
using System;

public class Player : MonoBehaviour, IDamagable, IHealable
{
    // Prefabs used to show visual feedback when the player takes damage or heals.
    [SerializeField] private DamageFeedback damagePrefab;
    [SerializeField] private DamageFeedback healthPrefab;

    // Visual effects spawned when the player is damaged or healed.
    [SerializeField] private GameObject cloudPrefab;
    [SerializeField] private GameObject healPrefab;

    // Current health is changed through Damage and Heal.
    public int health { get; set; }

    public int stamina;

    // Maximum health is exposed through a read-only property for other scripts.
    public int maxHealth;
    public int MaxHealth => maxHealth;

    // Events allow UI and other systems to react without Player needing direct references.
    public Action OnDamageTaken;
    public Action OnHealed;
    //public Action OnStamindaUsed;
    public Action OnPlayerDeath;

    public int level;

    private void Awake()
    {
        // Start every battle with full health.
        health = maxHealth;
    }

    // Reduces health, creates damage feedback, and notifies subscribed systems.
    public void Damage(int damage)
    {
        // Dead players cannot take additional damage.
        if (health <= 0) return;

        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);

        // Spawn the floating damage number above the player.
        DamageFeedback damageFeedback = Instantiate(damagePrefab, transform.position + Vector3.up, Quaternion.identity);
        damageFeedback.DisplayDamage(damage);

        Instantiate(cloudPrefab, transform.position, Quaternion.identity);

        OnDamageTaken?.Invoke();
        SFXPlayer.Instance.PlaySFX(6, 1);

        // Only notify other systems when health has reached zero.
        if (health <= 0) OnPlayerDeath?.Invoke();
    }

    // Restores health, creates healing feedback, and updates subscribed systems.
    public void Heal(int points)
    {
        // A defeated player cannot be healed by this method.
        if (health <= 0) return;

        health += points;
        health = Mathf.Clamp(health, 0, maxHealth);

        // Spawn the floating healing number above the player.
        DamageFeedback damageFeedback = Instantiate(healthPrefab, transform.position + Vector3.up, Quaternion.identity);
        damageFeedback.DisplayDamage(points);

        Instantiate(healPrefab, transform.position, Quaternion.identity);

        OnHealed?.Invoke();
        SFXPlayer.Instance.PlaySFX(7, 1);
    }
}