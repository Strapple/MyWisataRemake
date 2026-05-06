using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [Header("Settings")]
    public int reward = 500;
    public float lifetime = 4f;

    [Header("Animation")]
    public float bobSpeed = 2f;
    public float bobHeight = 0.2f;

    private Vector3 startPos;
    private float bobTimer = 0f;
    private bool isCollected = false;

    void Start()
    {
        startPos = transform.position;

        // Tambah Collider jika belum ada
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (col == null)
        {
            col = gameObject.AddComponent<BoxCollider2D>();
            col.size = new Vector2(1f, 1f);
        }
        col.isTrigger = true; // Trigger agar tidak nabrak player
    }

    void Update()
    {
        if (isCollected) return;

        // Animasi bobbing
        bobTimer += Time.deltaTime * bobSpeed;
        float offsetY = Mathf.Sin(bobTimer) * bobHeight;
        transform.position = startPos + new Vector3(0, offsetY, 0);
    }

    void OnMouseDown()
    {
        if (isCollected) return;

        Collect();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Opsional: Player bisa ambil dengan menyentuh
        if (other.CompareTag("Player") && !isCollected)
        {
            Collect();
        }
    }

    void Collect()
    {
        isCollected = true;

        // Tambah uang
        if (GameManager.Instance != null)
        {
            GameManager.Instance.TambahUang(reward);
        }

        // Efek visual (opsional)
        StartCoroutine(CollectAnimation());
    }

    System.Collections.IEnumerator CollectAnimation()
    {
        float duration = 0.2f;
        float elapsed = 0f;
        Vector3 originalScale = transform.localScale;

        // Zoom out + fade
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.localScale = originalScale * (1f + t * 0.5f);
            yield return null;
        }

        Destroy(gameObject);
    }
}