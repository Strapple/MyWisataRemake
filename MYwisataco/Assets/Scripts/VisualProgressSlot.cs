using UnityEngine;

public class VisualProgressSlot : MonoBehaviour
{
    [Header("Slot Settings")]
    public float ratingThreshold = 2.3f;

    private SpriteRenderer spriteRenderer;
    private bool hasAppeared = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }

    void Update()
    {
        if (!hasAppeared && GameManager.Instance.rating >= ratingThreshold)
        {
            hasAppeared = true;
            spriteRenderer.enabled = true;
            StartCoroutine(PopAnimation());
        }
    }

    System.Collections.IEnumerator PopAnimation()
    {
        transform.localScale = Vector3.zero;
        float duration = 0.3f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            yield return null;
        }

        transform.localScale = Vector3.one;
    }
}