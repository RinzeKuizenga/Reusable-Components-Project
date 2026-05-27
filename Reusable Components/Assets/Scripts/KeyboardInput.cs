using System.IO;
using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    [SerializeField] KeyCode left;
    [SerializeField] KeyCode right;
    [SerializeField] KeyCode down;
    [SerializeField] KeyCode up;

    Vector2 direction;
 
    public Vector2 GetInput()
    {
        direction = Vector2.zero;   
        if (Input.GetKey(left))
        {
            direction.x = -1;
        }
        if (Input.GetKey(right))
        {
            direction.x = 1;
        }
        if (Input.GetKey(down))
        {
            direction.y = -1;
        }
        if (Input.GetKey(up))
        {
            direction.y = 1;
        }
        return direction.normalized;
    }
}
