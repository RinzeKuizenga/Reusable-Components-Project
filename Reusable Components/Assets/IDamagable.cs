using UnityEngine;

public interface IDamagable { 
    int health { get; set; }
    void Damage(int damage);
    
}
