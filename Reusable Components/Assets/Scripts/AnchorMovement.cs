using UnityEngine;
using System;

public class AnchorMovement : MonoBehaviour
{
    private float speed = 16f;

    public Vector2 target;

    public bool isMoving = false;

    public Action onFinishedMoving;
    private void Update()
    {
        if (!isMoving)
        {
            return;
        }
        transform.position = Vector2.Lerp(transform.position, target, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, target) < 0.02f)
        {
            transform.position = target;
            isMoving = false;
            onFinishedMoving?.Invoke();
        }
    }

    public void MoveTo(Vector2 newTarget, int newSpeed)
    {
        target = newTarget;
        isMoving = true;
        speed = newSpeed;   
    }

    public void MoveTo(Transform targetTransform, int newSpeed)
    {
        target = targetTransform.position;
        isMoving = true;
        speed = newSpeed;
    }
}
