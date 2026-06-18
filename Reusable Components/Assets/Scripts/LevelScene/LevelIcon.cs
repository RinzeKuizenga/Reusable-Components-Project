using UnityEngine;
using System;

public class LevelIcon : MonoBehaviour
{
    public bool isCompleted;
    public SpriteRenderer spriteRenderer;
    public Transform transform;

    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite completedSprite;
    [SerializeField] private Animator animator;

    private void Awake()
    {

        animator = GetComponent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        transform = GetComponent<Transform>();

    }

    public void SetCompleted(bool completed)
    {
        Debug.Log($"{name} SetCompleted({completed})");
        isCompleted = completed;
        spriteRenderer.sprite = isCompleted ? completedSprite : normalSprite;
    }

    public void StopAnimator()
    {
        if (animator != null) animator.enabled = false;
    }
}
