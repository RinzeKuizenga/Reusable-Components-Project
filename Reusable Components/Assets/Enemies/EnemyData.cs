using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Sample")]
public class EnemyData : ScriptableObject
{
    public string name;
    public int maxhealth;
    public Sprite sprite;
}
