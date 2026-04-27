using UnityEngine;
using UnityEngine.XR;

public class FreeMovement : MonoBehaviour
{
    Rigidbody2D rb => GetComponent<Rigidbody2D>();
    [SerializeField] float movementSpeed;
    [SerializeField] float verticalSpeed;

    [SerializeField] float minX, maxX;
    [SerializeField] float minY, maxY;
    public void Move(Vector2 direction)
    {
        Vector2 clampedPosition = rb.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);

        Vector2 directionChange = new Vector2(direction.x, direction.y * verticalSpeed);
        rb.linearVelocity = directionChange * movementSpeed;


        rb.position = clampedPosition;  
    }
}
