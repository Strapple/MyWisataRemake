using UnityEngine;
using System.Collections.Generic;

public class TurisSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject turisPrefab;
    public Transform[] spawnPoints;
    public Transform[] waypoints;
    public float spawnInterval = 5f;

    private float spawnTimer = 0f;
    private List<GameObject> turisAktif = new List<GameObject>();

    void Start()
    {
        // Cek apakah semua sudah terisi
        if (turisPrefab == null)
            Debug.LogError("Turis Prefab belum di-assign!");
        if (spawnPoints == null || spawnPoints.Length == 0)
            Debug.LogError("Spawn Points kosong!");
        if (waypoints == null || waypoints.Length == 0)
            Debug.LogError("Waypoints kosong!");
    }

    void Update()
    {
        if (GameManager.Instance == null) return;

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval && turisAktif.Count < GameManager.Instance.maxTuris)
        {
            spawnTimer = 0f;
            SpawnTuris();
        }

        // Bersihkan list dari turis yang sudah null
        turisAktif.RemoveAll(t => t == null);
    }

    void SpawnTuris()
    {
        if (turisPrefab == null || spawnPoints.Length == 0)
        {
            Debug.LogError("Tidak bisa spawn: Prefab atau Spawn Points kosong!");
            return;
        }

        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("Waypoints di TurisSpawner kosong! Isi di Inspector!");
            return;
        }

        // Pilih spawn point random
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject turis = Instantiate(turisPrefab, spawnPoint.position, Quaternion.identity);
        turis.name = "Turis_" + turisAktif.Count;

        TurisController controller = turis.GetComponent<TurisController>();
        if (controller != null)
        {
            controller.waypoints = waypoints;
            controller.OnTurisSelesai += HandleTurisSelesai;
            Debug.Log($"[Spawner] Waypoints di-assign ke {turis.name}, jumlah: {waypoints.Length}");
        }
        else
        {
            Debug.LogError("TurisController tidak ditemukan di prefab!");
        }

        turisAktif.Add(turis);
    }

    void HandleTurisSelesai(GameObject turis)
    {
        turisAktif.Remove(turis);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.TambahUang(GameManager.Instance.pendapatanPerTuris);
        }

        Destroy(turis);
    }

    public int GetTurisAktif()
    {
        turisAktif.RemoveAll(t => t == null);
        return turisAktif.Count;
    }
}