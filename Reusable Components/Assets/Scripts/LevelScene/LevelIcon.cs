using UnityEngine;

public class LevelIcon : MonoBehaviour
{
    public bool isCompleted;
    public SpriteRenderer spriteRenderer;
    public Transform transform;

    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite completedSprite;
    [SerializeField] private Animator animator;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
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
