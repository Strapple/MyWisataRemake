using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton Pattern
    public static GameManager Instance;

    // ========== DATA PEMAIN ==========
    public int uang = 100000;
    public float rating = 2.0f;
    public float kebersihan = 80f;

    // ========== STATUS SLOT LUAR (INTERAKTIF) ==========
    public bool slotLuarA_Dibeli = false;
    public bool slotLuarB_Dibeli = false;
    public Sprite slotLuarA_Sprite = null;
    public Sprite slotLuarB_Sprite = null;

    // ========== STATUS SLOT DALAM ==========
    public bool slotDalam1_Dibeli = false;
    public bool slotDalam2_Dibeli = false;
    public bool slotDalam3_Dibeli = false;
    public bool slotDalam4_Dibeli = false;
    public bool slotDalam5_Dibeli = false;

    // ========== SPRITE SLOT DALAM (UNTUK LOAD) ==========
    public string slotDalam1_SpriteName = "";
    public string slotDalam2_SpriteName = "";
    public string slotDalam3_SpriteName = "";
    public string slotDalam4_SpriteName = "";
    public string slotDalam5_SpriteName = "";

    // ========== STATUS BANGUNAN ==========
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
        // Singleton Setup
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
        // Update kebersihan setiap detik
        kebersihanUpdateTimer += Time.deltaTime;
        if (kebersihanUpdateTimer >= 1.0f)
        {
            kebersihanUpdateTimer = 0f;
            UpdateKebersihan();
        }

        // Update rating setiap 5 detik
        ratingUpdateTimer += Time.deltaTime;
        if (ratingUpdateTimer >= 5.0f)
        {
            ratingUpdateTimer = 0f;
            UpdateRating();
        }

        // Update max turis berdasarkan rating
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
        {
            rating = Mathf.Min(3.0f, rating + RATING_NAIK_RATE);
        }
        else if (kebersihan < 30f)
        {
            rating = Mathf.Max(0f, rating - RATING_TURUN_RATE);
        }

        OnDataUpdated?.Invoke();
    }

    void UpdateMaxTuris()
    {
        if (rating >= 3.0f) maxTuris = 8;
        else if (rating >= 2.5f) maxTuris = 6;
        else if (rating >= 2.0f) maxTuris = 4;
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
        rating = Mathf.Min(3.0f, rating + jumlah);
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