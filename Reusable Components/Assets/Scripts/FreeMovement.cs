using UnityEngine;
using UnityEngine.XR;

public class FreeMovement : MonoBehaviour
{
    // Retrieves the Rigidbody2D attached to this GameObject for movement control.
    Rigidbody2D rb => GetComponent<Rigidbody2D>();

    // Movement settings 
    [SerializeField] float movementSpeed = 3f;
    [SerializeField] float verticalSpeed = 0.9f;

    [SerializeField] float minX = -5.59f;
    [SerializeField] float maxX = 1.42f;
    [SerializeField] float minY = -3.8f;
    [SerializeField] float maxY = 0.93f;

    // Moves the character in the provided direction while keeping it inside the movement bounds.
    public void Move(Vector2 direction)
    {
        // Read the current position before applying the movement boundaries.
        Vector2 clampedPosition = rb.position;

        // Keep the character within the configured horizontal and vertical limits.
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);

        // Apply a separate multiplier to vertical movement for the battle grid.
        Vector2 directionChange = new Vector2(direction.x, direction.y * verticalSpeed);

        // Move the Rigidbody2D using its linear velocity.
        rb.linearVelocity = directionChange * movementSpeed;

        // Apply the clamped position after updating movement.
        rb.position = clampedPosition;
    }

    // Immediately stops all movement by clearing the Rigidbody2D velocity.
    public void StopMove()
    {
        rb.linearVelocity = Vector2.zero;
    }
}