using UnityEngine;
using System.Collections;

public class CoinSpawner : MonoBehaviour
{
    [Header("Coin Settings")]
    public GameObject coinPrefab;
    public float spawnInterval = 12f;      // Setiap 12 detik
    public float coinLifetime = 4f;        // Koin hilang setelah 4 detik
    public int coinReward = 500;           // Uang yang didapat

    [Header("Slot Reference")]
    public DecorationSlot_Luar slotScript;

    private bool isSpawning = false;

    void Start()
    {
        // Cek status slot (dipanggil setelah slot dibeli)
        StartCoroutine(CheckAndStartSpawning());
    }

    IEnumerator CheckAndStartSpawning()
    {
        // Tunggu sampai slot dibeli
        while (slotScript != null && !slotScript.IsPurchased())
        {
            yield return new WaitForSeconds(1f);
        }

        // Mulai spawn koin
        isSpawning = true;
        StartCoroutine(SpawnCoinRoutine());
    }

    IEnumerator SpawnCoinRoutine()
    {
        while (isSpawning)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (coinPrefab != null)
            {
                SpawnCoin();
            }
        }
    }

    void SpawnCoin()
    {
        // Spawn koin di atas slot
        Vector3 spawnPos = transform.position + new Vector3(0, 1.5f, 0);
        GameObject coin = Instantiate(coinPrefab, spawnPos, Quaternion.identity);

        // Setup koin
        CoinPickup coinScript = coin.GetComponent<CoinPickup>();
        if (coinScript != null)
        {
            coinScript.reward = coinReward;
            coinScript.lifetime = coinLifetime;
        }

        // Hancurkan koin setelah waktu tertentu
        Destroy(coin, coinLifetime);
    }

    public void StopSpawning()
    {
        isSpawning = false;
    }
}