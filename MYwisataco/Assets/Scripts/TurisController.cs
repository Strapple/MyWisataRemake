using UnityEngine;

public class TurisController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public Transform[] waypoints;

    public System.Action<GameObject> OnTurisSelesai;

    private int currentWaypoint = 0;

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            // Diam jika waypoints kosong
            return;
        }

        Transform target = waypoints[currentWaypoint];

        if (target == null) return;

        // Gerak ke target
        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );

        // Cek apakah sudah sampai
        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            currentWaypoint++;

            if (currentWaypoint >= waypoints.Length)
            {
                // Sampai tujuan akhir
                OnTurisSelesai?.Invoke(gameObject);
            }
        }
    }
}