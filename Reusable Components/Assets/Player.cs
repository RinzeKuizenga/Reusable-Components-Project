using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Player : MonoBehaviour, IDamagable
{
    [SerializeField] private DamageFeedback damagePrefab;
    public int health { get; set; }

    [SerializeField] private int maxHealth;

    public void Start()
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
    }
}
