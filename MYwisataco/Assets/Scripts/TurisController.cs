using UnityEngine;

public class TurisController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public Transform[] waypoints;

    [Header("Animation")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    public System.Action<GameObject> OnTurisSelesai;

    private int currentWaypoint = 0;
    private Vector2 previousPosition;
    private float moveX;
    private bool isWalking;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        previousPosition = transform.position;
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0)
            return;

        Transform target = waypoints[currentWaypoint];
        if (target == null) return;

        // Gerak ke target
        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );

        // Deteksi arah dari perubahan posisi X (seperti player)
        float deltaX = transform.position.x - previousPosition.x;

        if (deltaX > 0.01f)
            moveX = 1f;
        else if (deltaX < -0.01f)
            moveX = -1f;

        isWalking = Mathf.Abs(deltaX) > 0.001f;

        // Update animasi
        UpdateAnimation();

        // Simpan posisi untuk frame berikutnya
        previousPosition = transform.position;

        // Cek sampai waypoint
        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            currentWaypoint++;

            if (currentWaypoint >= waypoints.Length)
            {
                OnTurisSelesai?.Invoke(gameObject);
            }
        }
    }

    void UpdateAnimation()
    {
        if (animator == null) return;

        animator.SetBool("IsWalking", isWalking);
        animator.SetFloat("MoveX", moveX);
    }
}