using UnityEngine;

public interface IHealable
{
    int health { get; set; }
    int MaxHealth { get; }
    void Heal(int points);



}