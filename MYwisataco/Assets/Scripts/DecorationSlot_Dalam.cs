using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DecorationSlot_Dalam : MonoBehaviour
{
    [Header("Slot Settings")]
    public string slotID = "Dalam_1";
    public string itemName = "Lukisan";
    public int price = 10000;
    public float ratingBonus = 0.2f;
    public Sprite purchasedSprite;

    [Header("UI Popup")]
    public GameObject popupPanel;
    public TextMeshProUGUI txtPopupTitle;
    public TextMeshProUGUI txtPopupPrice;
    public Button btnYa;
    public Button btnTidak;

    [Header("Interaction")]
    public float interactionRange = 2f;
    public GameObject interactHint;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private bool isPurchased = false;
    private bool playerInRange = false;
    private Transform player;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        LoadPurchaseState();

        if (popupPanel != null)
            popupPanel.SetActive(false);
        if (interactHint != null)
            interactHint.SetActive(false);

        // Setup tombol popup
        if (btnYa != null)
        {
            btnYa.onClick.RemoveAllListeners();
            btnYa.onClick.AddListener(OnBeliClicked);
        }
        if (btnTidak != null)
        {
            btnTidak.onClick.RemoveAllListeners();
            btnTidak.onClick.AddListener(OnBatalClicked);
        }
    }

    void Update()
    {
        if (isPurchased || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        playerInRange = distance <= interactionRange;

        if (interactHint != null)
        {
            interactHint.SetActive(playerInRange);
        }

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ShowPurchasePopup();
        }
    }

    void ShowPurchasePopup()
    {
        if (popupPanel == null) return;

        txtPopupTitle.text = $"Beli {itemName}?";
        txtPopupPrice.text = $"Harga: Rp {price:N0}\nBonus Rating: +{ratingBonus}";
        popupPanel.SetActive(true);
    }

    void LoadPurchaseState()
    {
        if (GameManager.Instance == null) return;

        bool purchased = false;
        switch (slotID)
        {
            case "Dalam_1": purchased = GameManager.Instance.slotDalam1_Dibeli; break;
            case "Dalam_2": purchased = GameManager.Instance.slotDalam2_Dibeli; break;
            case "Dalam_3": purchased = GameManager.Instance.slotDalam3_Dibeli; break;
            case "Dalam_4": purchased = GameManager.Instance.slotDalam4_Dibeli; break;
            case "Dalam_5": purchased = GameManager.Instance.slotDalam5_Dibeli; break;
        }

        if (purchased)
        {
            SetPurchased();
        }
    }

    void SavePurchaseState()
    {
        if (GameManager.Instance == null) return;

        switch (slotID)
        {
            case "Dalam_1": GameManager.Instance.slotDalam1_Dibeli = true; break;
            case "Dalam_2": GameManager.Instance.slotDalam2_Dibeli = true; break;
            case "Dalam_3": GameManager.Instance.slotDalam3_Dibeli = true; break;
            case "Dalam_4": GameManager.Instance.slotDalam4_Dibeli = true; break;
            case "Dalam_5": GameManager.Instance.slotDalam5_Dibeli = true; break;
        }
    }

    void OnBeliClicked()
    {
        if (GameManager.Instance == null) return;

        if (GameManager.Instance.KurangiUang(price))
        {
            GameManager.Instance.TambahRating(ratingBonus);
            SavePurchaseState();
            SetPurchased();
        }
        popupPanel.SetActive(false);
    }

    void OnBatalClicked()
    {
        popupPanel.SetActive(false);
    }

    void SetPurchased()
    {
        isPurchased = true;
        if (purchasedSprite != null)
            spriteRenderer.sprite = purchasedSprite;
        if (boxCollider != null)
            boxCollider.enabled = false;
        if (interactHint != null)
            interactHint.SetActive(false);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}