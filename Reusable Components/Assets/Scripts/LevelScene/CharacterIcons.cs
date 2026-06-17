using UnityEngine;

public class CharacterIcons : MonoBehaviour
{
    AnchorMovement anchorMovement;

    [SerializeField] private Vector2 offset = new Vector2(-0.5f, 2);


    private void Start()
    {
        anchorMovement = GetComponent<AnchorMovement>();
        anchorMovement.onFinishedMoving +=
    }

    public void MoveIcons(Vector2 target)
    {
        anchorMovement.MoveTo(target + offset, 4);
    }
}
