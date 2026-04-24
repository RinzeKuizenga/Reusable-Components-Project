using UnityEngine;
using UnityEngine.XR;

public class FreeMovement : MonoBehaviour
{
    Rigidbody2D rb => GetComponent<Rigidbody2D>();
    [SerializeField] float movementSpeed;
    [SerializeField] float verticalSpeed;


    public void Move(Vector2 direction)
    {
        Vector2 directionChange = new Vector2(direction.x, direction.y * verticalSpeed);
        rb.linearVelocity = directionChange * movementSpeed;
    }
}
