using UnityEngine;

public class GridTile : MonoBehaviour
{
    public bool IsDangerous;

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite safeSprite;
    [SerializeField] private Sprite dangerSprite;
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private int damage = 10;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetDangerous(bool dangerous)
    {
        IsDangerous = dangerous;

        spriteRenderer.sprite =
            dangerous ? dangerSprite : safeSprite;
    }

    public void SetIdle()
    {
        spriteRenderer.sprite = idleSprite;
    }

    public void DamageAnythingInside()
    {
        if (!IsDangerous) return;

        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0f);

        foreach (Collider2D hit in hits)
        {
            IDamagable target = hit.GetComponent<IDamagable>();

            if (target != null)
            {
                target.Damage(damage);
            }
        }
    }
}


