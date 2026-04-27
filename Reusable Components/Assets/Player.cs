using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDamagable
{
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
    }
}
