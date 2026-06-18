using System.Collections;
using UnityEngine;

public class ControlHint : MonoBehaviour
{
    [SerializeField] private float followDuration = 2f;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private Vector3 offset = new Vector3(0, 2f, 0);

    private Transform target;
    private SpriteRenderer[] spriteRenderers;

    public void Setup(Transform player)
    {
        target = player;
    }

    private void Awake()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        StartCoroutine(HintCoroutine());
    }

    private void Update()
    {
        if (target == null) return;

        transform.position = target.position + offset;
    }

    private IEnumerator HintCoroutine()
    {
        yield return new WaitForSeconds(followDuration);

        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);

            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                Color color = spriteRenderer.color;
                color.a = alpha;
                spriteRenderer.color = color;
            }

            yield return null;
        }

        Destroy(gameObject);
    }
}