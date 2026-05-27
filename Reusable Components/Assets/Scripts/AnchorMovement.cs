using UnityEngine;

public class AnchorMovement : MonoBehaviour
{
    private float speed = 16f;

    public Vector2 target;

    public bool isMoving = false;


    private void Update()
    {
        if (!isMoving)
        {
            return;
        }
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, target) < 0.02f)
        {
            isMoving = false;
        }
    }

    public void MoveTo(Vector2 newTarget, int newSpeed)
    {
        target = newTarget;
        isMoving = true;
        speed = newSpeed;   
    }
}
