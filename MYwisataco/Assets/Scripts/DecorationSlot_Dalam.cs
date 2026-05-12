using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class ItemOption
{
    public string itemName;
    public Sprite itemSprite;
    public int price;
    public float ratingBonus;
}

public class DecorationSlot_Dalam : MonoBehaviour
{
    [Header("Slot Settings")]
    public string slotID = "Dalam_1";

    [Header("3 Opsi Item")]
    public ItemOption option1;
    public ItemOption option2;
    public ItemOption option3;

    [Header("UI Popup")]
    public GameObject popupPanel;
    public TextMeshProUGUI txtPopupTitle;

    public Image imgItem1;
    public TextMeshProUGUI txtName1;
    public TextMeshProUGUI txtPrice1;
    public TextMeshProUGUI txtBonus1;
    public Button btnBeli1;

    public Image imgItem2;
    public TextMeshProUGUI txtName2;
    public TextMeshProUGUI txtPrice2;
    public TextMeshProUGUI txtBonus2;
    public Button btnBeli2;

    public Image imgItem3;
    public TextMeshProUGUI txtName3;
    public TextMeshProUGUI txtPrice3;
    public TextMeshProUGUI txtBonus3;
    public Button btnBeli3;

    public Button btnTutup;

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

        if (popupPanel != null) popupPanel.SetActive(false);
        if (interactHint != null) interactHint.SetActive(false);

        if (btnBeli1 != null) btnBeli1.onClick.AddListener(() => OnBeliClicked(1));
        if (btnBeli2 != null) btnBeli2.onClick.AddListener(() => OnBeliClicked(2));
        if (btnBeli3 != null) btnBeli3.onClick.AddListener(() => OnBeliClicked(3));
        if (btnTutup != null) btnTutup.onClick.AddListener(OnTutupClicked);
    }

    void Update()
    {
        if (isPurchased || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        playerInRange = distance <= interactionRange;

        if (interactHint != null) interactHint.SetActive(playerInRange);

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
            ShowPopup();
    }

    void ShowPopup()
    {
        if (popupPanel == null) return;
        SetupOpsiUI(option1, imgItem1, txtName1, txtPrice1, txtBonus1);
        SetupOpsiUI(option2, imgItem2, txtName2, txtPrice2, txtBonus2);
        SetupOpsiUI(option3, imgItem3, txtName3, txtPrice3, txtBonus3);
        popupPanel.SetActive(true);
    }

    void SetupOpsiUI(ItemOption option, Image img, TextMeshProUGUI name, TextMeshProUGUI price, TextMeshProUGUI bonus)
    {
        if (img != null && option.itemSprite != null) img.sprite = option.itemSprite;
        if (name != null) name.text = option.itemName;
        if (price != null) price.text = $"Rp {option.price:N0}";
        if (bonus != null) bonus.text = $"+{option.ratingBonus} ⭐";
    }

    void OnBeliClicked(int opsiKe)
    {
        ItemOption selectedOption = null;
        switch (opsiKe)
        {
            case 1: selectedOption = option1; break;
            case 2: selectedOption = option2; break;
            case 3: selectedOption = option3; break;
        }

        if (selectedOption == null || GameManager.Instance == null) return;

        if (GameManager.Instance.KurangiUang(selectedOption.price))
        {
            GameManager.Instance.TambahRating(selectedOption.ratingBonus);
            SavePurchaseState(selectedOption.itemSprite, selectedOption.itemName);

            if (selectedOption.itemSprite != null)
                spriteRenderer.sprite = selectedOption.itemSprite;

            isPurchased = true;
            if (boxCollider != null) boxCollider.enabled = false;
            if (interactHint != null) interactHint.SetActive(false);
        }

        popupPanel.SetActive(false);
    }

    void OnTutupClicked()
    {
        popupPanel.SetActive(false);
    }

    void LoadPurchaseState()
    {
        if (GameManager.Instance == null) return;

        bool purchased = false;
        Sprite savedSprite = null;

        switch (slotID)
        {
            case "Dalam_1": purchased = GameManager.Instance.slotDalam1_Dibeli; savedSprite = GameManager.Instance.slotDalam1_Sprite; break;
            case "Dalam_2": purchased = GameManager.Instance.slotDalam2_Dibeli; savedSprite = GameManager.Instance.slotDalam2_Sprite; break;
            case "Dalam_3": purchased = GameManager.Instance.slotDalam3_Dibeli; savedSprite = GameManager.Instance.slotDalam3_Sprite; break;
            case "Dalam_4": purchased = GameManager.Instance.slotDalam4_Dibeli; savedSprite = GameManager.Instance.slotDalam4_Sprite; break;
            case "Dalam_5": purchased = GameManager.Instance.slotDalam5_Dibeli; savedSprite = GameManager.Instance.slotDalam5_Sprite; break;
            case "Dalam_6": purchased = GameManager.Instance.slotDalam6_Dibeli; savedSprite = GameManager.Instance.slotDalam6_Sprite; break;
            case "Dalam_7": purchased = GameManager.Instance.slotDalam7_Dibeli; savedSprite = GameManager.Instance.slotDalam7_Sprite; break;
            case "Dalam_8": purchased = GameManager.Instance.slotDalam8_Dibeli; savedSprite = GameManager.Instance.slotDalam8_Sprite; break;
            case "Miniatur_1": purchased = GameManager.Instance.slotMiniatur1_Dibeli; savedSprite = GameManager.Instance.slotMiniatur1_Sprite; break;
            case "Lukisan_1": purchased = GameManager.Instance.slotLukisan1_Dibeli; savedSprite = GameManager.Instance.slotLukisan1_Sprite; break;
            case "Lukisan_2": purchased = GameManager.Instance.slotLukisan2_Dibeli; savedSprite = GameManager.Instance.slotLukisan2_Sprite; break;
            case "Lukisan_3": purchased = GameManager.Instance.slotLukisan3_Dibeli; savedSprite = GameManager.Instance.slotLukisan3_Sprite; break;
            case "Lukisan_4": purchased = GameManager.Instance.slotLukisan4_Dibeli; savedSprite = GameManager.Instance.slotLukisan4_Sprite; break;
            case "Lukisan_5": purchased = GameManager.Instance.slotLukisan5_Dibeli; savedSprite = GameManager.Instance.slotLukisan5_Sprite; break;
            case "Lukisan_6": purchased = GameManager.Instance.slotLukisan6_Dibeli; savedSprite = GameManager.Instance.slotLukisan6_Sprite; break;
        }

        if (purchased && savedSprite != null)
        {
            spriteRenderer.sprite = savedSprite;
            isPurchased = true;
            if (boxCollider != null) boxCollider.enabled = false;
            if (interactHint != null) interactHint.SetActive(false);
        }
    }

    void SavePurchaseState(Sprite spriteToSave, string itemName)
    {
        if (GameManager.Instance == null) return;

        switch (slotID)
        {
            case "Dalam_1": GameManager.Instance.slotDalam1_Dibeli = true; GameManager.Instance.slotDalam1_Sprite = spriteToSave; GameManager.Instance.slotDalam1_ItemName = itemName; break;
            case "Dalam_2": GameManager.Instance.slotDalam2_Dibeli = true; GameManager.Instance.slotDalam2_Sprite = spriteToSave; GameManager.Instance.slotDalam2_ItemName = itemName; break;
            case "Dalam_3": GameManager.Instance.slotDalam3_Dibeli = true; GameManager.Instance.slotDalam3_Sprite = spriteToSave; GameManager.Instance.slotDalam3_ItemName = itemName; break;
            case "Dalam_4": GameManager.Instance.slotDalam4_Dibeli = true; GameManager.Instance.slotDalam4_Sprite = spriteToSave; GameManager.Instance.slotDalam4_ItemName = itemName; break;
            case "Dalam_5": GameManager.Instance.slotDalam5_Dibeli = true; GameManager.Instance.slotDalam5_Sprite = spriteToSave; GameManager.Instance.slotDalam5_ItemName = itemName; break;
            case "Dalam_6": GameManager.Instance.slotDalam6_Dibeli = true; GameManager.Instance.slotDalam6_Sprite = spriteToSave; GameManager.Instance.slotDalam6_ItemName = itemName; break;
            case "Dalam_7": GameManager.Instance.slotDalam7_Dibeli = true; GameManager.Instance.slotDalam7_Sprite = spriteToSave; GameManager.Instance.slotDalam7_ItemName = itemName; break;
            case "Dalam_8": GameManager.Instance.slotDalam8_Dibeli = true; GameManager.Instance.slotDalam8_Sprite = spriteToSave; GameManager.Instance.slotDalam8_ItemName = itemName; break;
            case "Miniatur_1": GameManager.Instance.slotMiniatur1_Dibeli = true; GameManager.Instance.slotMiniatur1_Sprite = spriteToSave; GameManager.Instance.slotMiniatur1_ItemName = itemName; break;
            case "Lukisan_1": GameManager.Instance.slotLukisan1_Dibeli = true; GameManager.Instance.slotLukisan1_Sprite = spriteToSave; GameManager.Instance.slotLukisan1_ItemName = itemName; break;
            case "Lukisan_2": GameManager.Instance.slotLukisan2_Dibeli = true; GameManager.Instance.slotLukisan2_Sprite = spriteToSave; GameManager.Instance.slotLukisan2_ItemName = itemName; break;
            case "Lukisan_3": GameManager.Instance.slotLukisan3_Dibeli = true; GameManager.Instance.slotLukisan3_Sprite = spriteToSave; GameManager.Instance.slotLukisan3_ItemName = itemName; break;
            case "Lukisan_4": GameManager.Instance.slotLukisan4_Dibeli = true; GameManager.Instance.slotLukisan4_Sprite = spriteToSave; GameManager.Instance.slotLukisan4_ItemName = itemName; break;
            case "Lukisan_5": GameManager.Instance.slotLukisan5_Dibeli = true; GameManager.Instance.slotLukisan5_Sprite = spriteToSave; GameManager.Instance.slotLukisan5_ItemName = itemName; break;
            case "Lukisan_6": GameManager.Instance.slotLukisan6_Dibeli = true; GameManager.Instance.slotLukisan6_Sprite = spriteToSave; GameManager.Instance.slotLukisan6_ItemName = itemName; break;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}