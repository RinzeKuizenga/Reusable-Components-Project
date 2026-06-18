using UnityEngine;
using System;

public class CharacterIcons : MonoBehaviour
{
    public bool finishedMoving;

    private AnchorMovement anchorMovement;

    [SerializeField] private Vector2 offset = new Vector2(-0.5f, 2f);

    public Action OnFinishedMoving;

    private void Awake()
    {
        anchorMovement = GetComponent<AnchorMovement>();

        if (anchorMovement == null)
        {
            Debug.LogError("CharacterIcons could not find an AnchorMovement component.", gameObject);
            return;
        }

        anchorMovement.onFinishedMoving += HandleTransition;
        finishedMoving = false;
    }

    public void MoveIcons(Vector2 target)
    {
        if (anchorMovement == null) return;

        anchorMovement.MoveTo(target + offset, 2);
    }

    private void HandleTransition()
    {
        finishedMoving = true;
        OnFinishedMoving?.Invoke();
    }
}