using TMPro;
using UnityEngine;

public class DamageFeedback : MonoBehaviour
{
    private TextMeshPro damageText;

    bool isBig = false;
    private float holdTime = 0.8f;  
    private float elapsedTime = 0f;
    private void Awake()
    {
        damageText = GetComponent<TextMeshPro>();
        transform.localScale = Vector3.zero; 
    }

    public void DisplayDamage(int damage)
    {
        damageText.text = damage.ToString();
    }

    private void Update()
    {
        if (transform.localScale.x < 0.5f && !isBig)
        {
            transform.localScale += Vector3.one * 6f * Time.deltaTime;
        }

        if (transform.localScale.x >= 0.5f && !isBig)
        {
            isBig = true;
            elapsedTime = 0f; 
        }

        if (isBig && elapsedTime < holdTime)
        {
            elapsedTime += Time.deltaTime;
            return;  
        }

        if (transform.localScale.x > 0f && isBig)
        {
            transform.localScale -= Vector3.one * 6f * Time.deltaTime;
        }

    }

    private void Start()
    {
        transform.position += Vector3.up * 0.7f;
        transform.position += Vector3.left * 0.9f;
        Destroy(gameObject, 1.1f);
    }
}