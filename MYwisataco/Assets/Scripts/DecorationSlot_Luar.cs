using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DecorationSlot_Luar : MonoBehaviour
{
    [Header("Slot Settings")]
    public string slotID = "Luar_A";
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

        LoadPurchaseState();

        if (popupPanel != null)
            popupPanel.SetActive(false);

        if (btnYa != null)
            btnYa.onClick.AddListener(OnBeliClicked);
        if (btnTidak != null)
            btnTidak.onClick.AddListener(OnBatalClicked);
    }

    void OnMouseDown()
    {
        if (!isPurchased && popupPanel != null)
        {
            popupPanel.SetActive(true);
            txtPopupTitle.text = $"Beli {itemName}?";
            txtPopupPrice.text = $"Harga: Rp {price:N0}\nBonus Rating: +{ratingBonus}";
        }
    }

    void OnBeliClicked()
    {
        if (GameManager.Instance == null) return;

        if (GameManager.Instance.KurangiUang(price))
        {
            GameManager.Instance.TambahRating(ratingBonus);
            SavePurchaseState(purchasedSprite);
            SetPurchased();
        }

        popupPanel.SetActive(false);
    }

    void OnBatalClicked()
    {
        popupPanel.SetActive(false);
    }

    void LoadPurchaseState()
    {
        if (GameManager.Instance == null) return;

        bool purchased = false;
        Sprite savedSprite = null;

        if (slotID == "Luar_A")
        {
            purchased = GameManager.Instance.slotLuarA_Dibeli;
            savedSprite = GameManager.Instance.slotLuarA_Sprite;
        }
        else if (slotID == "Luar_B")
        {
            purchased = GameManager.Instance.slotLuarB_Dibeli;
            savedSprite = GameManager.Instance.slotLuarB_Sprite;
        }

        if (purchased)
        {
            if (savedSprite != null)
                purchasedSprite = savedSprite;

            if (purchasedSprite != null)
                spriteRenderer.sprite = purchasedSprite;

            isPurchased = true;
            if (boxCollider != null)
                boxCollider.enabled = false;

            CoinSpawner spawner = GetComponent<CoinSpawner>();
            if (spawner != null)
                spawner.enabled = true;
        }
    }

    void SavePurchaseState(Sprite spriteToSave)
    {
        if (GameManager.Instance == null) return;

        if (slotID == "Luar_A")
        {
            GameManager.Instance.slotLuarA_Dibeli = true;
            GameManager.Instance.slotLuarA_Sprite = spriteToSave;
            GameManager.Instance.slotLuarA_ItemName = itemName;
        }
        else if (slotID == "Luar_B")
        {
            GameManager.Instance.slotLuarB_Dibeli = true;
            GameManager.Instance.slotLuarB_Sprite = spriteToSave;
            GameManager.Instance.slotLuarB_ItemName = itemName;
        }
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

        CoinSpawner spawner = GetComponent<CoinSpawner>();
        if (spawner != null)
            spawner.enabled = true;
    }
}