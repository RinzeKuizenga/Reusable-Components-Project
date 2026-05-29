using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ControllerInput : MonoBehaviour, IInput
{
    [SerializeField] string horizontalAxis;
    [SerializeField] string verticalAxis;

    Vector2 direction;
    public Vector2 GetInput()
    {
        direction = Vector2.zero;

        direction.x = Input.GetAxis(horizontalAxis);
        direction.y = Input.GetAxis(verticalAxis);

        Debug.Log($"{horizontalAxis}");
        return direction.normalized;

    }


}
