using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton Pattern
    public static GameManager Instance;

    // ========== DATA PEMAIN ==========
    public int uang = 100000;
    public float rating = 2.0f;
    public float kebersihan = 80f;

    // ========== STATUS SLOT LUAR (FIX) ==========
    public bool slotAwalA_Dibeli = false;
    public bool slotAwalB_Dibeli = false;
    public Sprite slotAwalA_Sprite = null;
    public Sprite slotAwalB_Sprite = null;
    public string slotAwalA_ItemName = "";
    public string slotAwalB_ItemName = "";

    // ========== STATUS SLOT DALAM (3 OPSI) ==========
    public bool slotDalam1_Dibeli = false;
    public bool slotDalam2_Dibeli = false;
    public bool slotDalam3_Dibeli = false;
    public bool slotDalam4_Dibeli = false;
    public bool slotDalam5_Dibeli = false;
    public bool slotDalam6_Dibeli = false;
    public bool slotDalam7_Dibeli = false;
    public bool slotDalam8_Dibeli = false;

    public Sprite slotDalam1_Sprite = null;
    public Sprite slotDalam2_Sprite = null;
    public Sprite slotDalam3_Sprite = null;
    public Sprite slotDalam4_Sprite = null;
    public Sprite slotDalam5_Sprite = null;
    public Sprite slotDalam6_Sprite = null;
    public Sprite slotDalam7_Sprite = null;
    public Sprite slotDalam8_Sprite = null;

    public string slotDalam1_ItemName = "";
    public string slotDalam2_ItemName = "";
    public string slotDalam3_ItemName = "";
    public string slotDalam4_ItemName = "";
    public string slotDalam5_ItemName = "";
    public string slotDalam6_ItemName = "";
    public string slotDalam7_ItemName = "";
    public string slotDalam8_ItemName = "";

    // ========== STATUS MINIATUR (3 OPSI) ==========
    public bool slotMiniatur1_Dibeli = false;
    public Sprite slotMiniatur1_Sprite = null;
    public string slotMiniatur1_ItemName = "";

    // ========== STATUS LUKISAN (3 OPSI) ==========
    public bool slotLukisan1_Dibeli = false;
    public bool slotLukisan2_Dibeli = false;
    public bool slotLukisan3_Dibeli = false;
    public bool slotLukisan4_Dibeli = false;
    public bool slotLukisan5_Dibeli = false;
    public bool slotLukisan6_Dibeli = false;

    public Sprite slotLukisan1_Sprite = null;
    public Sprite slotLukisan2_Sprite = null;
    public Sprite slotLukisan3_Sprite = null;
    public Sprite slotLukisan4_Sprite = null;
    public Sprite slotLukisan5_Sprite = null;
    public Sprite slotLukisan6_Sprite = null;

    public string slotLukisan1_ItemName = "";
    public string slotLukisan2_ItemName = "";
    public string slotLukisan3_ItemName = "";
    public string slotLukisan4_ItemName = "";
    public string slotLukisan5_ItemName = "";
    public string slotLukisan6_ItemName = "";

    // ========== STATUS BANGUNAN (TOMBOL AKSI) ==========
    public bool toiletDibeli = false;
    public bool warungDibeli = false;
    public bool tongSampahDibeli = false;

    // ========== GAMEPLAY VARIABLES ==========
    public int pendapatanPerTuris = 2000;
    public float drainRateMultiplier = 1.0f;
    public int maxTuris = 3;

    // ========== KONSTANTA ==========
    private const float RATING_NAIK_RATE = 0.1f;
    private const float RATING_TURUN_RATE = 0.2f;
    private const float KEBERSIHAN_TURUN_RATE = 0.5f;

    // ========== TIMER ==========
    private float ratingUpdateTimer = 0f;
    private float kebersihanUpdateTimer = 0f;

    // ========== EVENT ==========
    public System.Action OnDataUpdated;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        kebersihanUpdateTimer += Time.deltaTime;
        if (kebersihanUpdateTimer >= 1.0f)
        {
            kebersihanUpdateTimer = 0f;
            UpdateKebersihan();
        }

        ratingUpdateTimer += Time.deltaTime;
        if (ratingUpdateTimer >= 5.0f)
        {
            ratingUpdateTimer = 0f;
            UpdateRating();
        }

        UpdateMaxTuris();
    }

    void UpdateKebersihan()
    {
        int turisAktif = FindObjectOfType<TurisSpawner>()?.GetTurisAktif() ?? 0;
        float pengurangan = turisAktif * KEBERSIHAN_TURUN_RATE * drainRateMultiplier;
        kebersihan = Mathf.Max(0, kebersihan - pengurangan);
        OnDataUpdated?.Invoke();
    }

    void UpdateRating()
    {
        if (kebersihan > 70f)
            rating = Mathf.Min(5.0f, rating + RATING_NAIK_RATE);
        else if (kebersihan < 30f)
            rating = Mathf.Max(0f, rating - RATING_TURUN_RATE);
        OnDataUpdated?.Invoke();
    }

    void UpdateMaxTuris()
    {
        if (rating >= 5.0f) maxTuris = 8;
        else if (rating >= 4.0f) maxTuris = 6;
        else if (rating >= 3.0f) maxTuris = 5;
        else if (rating >= 2.5f) maxTuris = 4;
        else if (rating >= 2.0f) maxTuris = 3;
        else maxTuris = 2;
    }

    // ========== PUBLIC METHODS ==========

    public bool KurangiUang(int jumlah)
    {
        if (uang >= jumlah)
        {
            uang -= jumlah;
            OnDataUpdated?.Invoke();
            return true;
        }
        return false;
    }

    public void TambahUang(int jumlah)
    {
        uang += jumlah;
        OnDataUpdated?.Invoke();
    }

    public void TambahRating(float jumlah)
    {
        rating = Mathf.Min(5.0f, rating + jumlah);
        OnDataUpdated?.Invoke();
    }

    public void TambahKebersihan(float jumlah)
    {
        kebersihan = Mathf.Min(100f, kebersihan + jumlah);
        OnDataUpdated?.Invoke();
    }

    public void BeliToilet()
    {
        if (!toiletDibeli && KurangiUang(25000))
        {
            toiletDibeli = true;
            TambahRating(0.5f);
        }
    }

    public void BeliWarung()
    {
        if (!warungDibeli && KurangiUang(35000))
        {
            warungDibeli = true;
            pendapatanPerTuris = 3500;
        }
    }

    public void BeliTongSampah()
    {
        if (!tongSampahDibeli && KurangiUang(50000))
        {
            tongSampahDibeli = true;
            drainRateMultiplier = 0.5f;
        }
    }

    public void KerahkanPetugas()
    {
        if (KurangiUang(5000))
        {
            TambahKebersihan(30f);
        }
    }
}