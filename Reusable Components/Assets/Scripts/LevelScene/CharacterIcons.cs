using UnityEngine;
using System;

public class CharacterIcons : MonoBehaviour
{
    public bool finishedMoving;
    AnchorMovement anchorMovement;

    [SerializeField] private Vector2 offset = new Vector2(-0.5f, 2);
    public Action OnFinishedMoving;


    private void Start()
    {
        anchorMovement = GetComponent<AnchorMovement>();
        anchorMovement.onFinishedMoving += HandleTransition;
        finishedMoving = false;
    }

    public void MoveIcons(Vector2 target)
    {
        anchorMovement.MoveTo(target + offset, 4);
    }

    void HandleTransition()
    {
        finishedMoving = true;
        OnFinishedMoving?.Invoke();
    }


}
