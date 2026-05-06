using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Animation")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;
    private Vector2 movement;
    private float moveX;
    private bool isWalking;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.bodyType = RigidbodyType2D.Dynamic;

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;

        moveX = movement.x;

        // ⬇️ PENTING: IsWalking = true HANYA jika ada input
        isWalking = movement.magnitude > 0.01f;

        UpdateAnimator();
    }

    void FixedUpdate()
    {
        rb.velocity = movement * moveSpeed;
    }

    void UpdateAnimator()
    {
        if (animator == null) return;

        animator.SetBool("IsWalking", isWalking);
        animator.SetFloat("MoveX", moveX);
    }
}