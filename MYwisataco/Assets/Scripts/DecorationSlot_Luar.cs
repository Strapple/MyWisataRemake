using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DecorationSlot_Luar : MonoBehaviour
{
    [Header("Slot Settings")]
    public string slotID = "Luar_A"; // "Luar_A" atau "Luar_B"
    public string itemName = "Papan Nama";
    public int price = 3000;
    public float ratingBonus = 0.1f;
    public Sprite purchasedSprite;

    [Header("UI Popup")]
    public GameObject popupPanel;
    public TextMeshProUGUI txtPopupTitle;
    public TextMeshProUGUI txtPopupPrice;
    public Button btnYa;
    public Button btnTidak;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private bool isPurchased = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        // Cek apakah sudah dibeli sebelumnya
        if (slotID == "Luar_A" && GameManager.Instance.slotLuarA_Dibeli)
        {
            SetPurchased();
        }
        else if (slotID == "Luar_B" && GameManager.Instance.slotLuarB_Dibeli)
        {
            SetPurchased();
        }

        // Sembunyikan popup awal
        if (popupPanel != null)
            popupPanel.SetActive(false);

        // Setup tombol popup
        if (btnYa != null)
            btnYa.onClick.AddListener(OnBeliClicked);
        if (btnTidak != null)
            btnTidak.onClick.AddListener(OnBatalClicked);
    }

    void OnMouseDown()
    {
        if (!isPurchased && popupPanel != null)
        {
            // Tampilkan popup
            popupPanel.SetActive(true);
            txtPopupTitle.text = $"Beli {itemName}?";
            txtPopupPrice.text = $"Harga: Rp {price:N0}\nBonus Rating: +{ratingBonus}";
        }
    }

    void OnBeliClicked()
    {
        if (GameManager.Instance.KurangiUang(price))
        {
            GameManager.Instance.TambahRating(ratingBonus);

            // Simpan status berdasarkan slot ID
            if (slotID == "Luar_A")
                GameManager.Instance.slotLuarA_Dibeli = true;
            else if (slotID == "Luar_B")
                GameManager.Instance.slotLuarB_Dibeli = true;

            SetPurchased();
        }

        popupPanel.SetActive(false);
    }

    void OnBatalClicked()
    {
        popupPanel.SetActive(false);
    }

    public bool IsPurchased()
    {
        return isPurchased;
    }
    void SetPurchased()
    {
        isPurchased = true;
        if (purchasedSprite != null)
            spriteRenderer.sprite = purchasedSprite;

        if (boxCollider != null)
            boxCollider.enabled = false;

        // Aktifkan CoinSpawner
        CoinSpawner spawner = GetComponent<CoinSpawner>();
        if (spawner != null)
        {
            spawner.enabled = true;
        }
    }
}