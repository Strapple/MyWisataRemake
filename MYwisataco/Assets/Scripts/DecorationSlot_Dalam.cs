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

    [Header("All Possible Sprites (untuk load)")]
    public Sprite[] allSprites; // ⬅️ Drag semua sprite opsi ke sini

    [Header("UI Popup")]
    public GameObject popupPanel;
    public TextMeshProUGUI txtPopupTitle;

    // Opsi 1
    public Image imgItem1;
    public TextMeshProUGUI txtName1;
    public TextMeshProUGUI txtPrice1;
    public TextMeshProUGUI txtBonus1;
    public Button btnBeli1;

    // Opsi 2
    public Image imgItem2;
    public TextMeshProUGUI txtName2;
    public TextMeshProUGUI txtPrice2;
    public TextMeshProUGUI txtBonus2;
    public Button btnBeli2;

    // Opsi 3
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

        if (popupPanel != null)
            popupPanel.SetActive(false);
        if (interactHint != null)
            interactHint.SetActive(false);

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

        if (interactHint != null)
        {
            interactHint.SetActive(playerInRange);
        }

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ShowPopup();
        }
    }

    void ShowPopup()
    {
        if (popupPanel == null) return;

        SetupOpsiUI(option1, imgItem1, txtName1, txtPrice1, txtBonus1, btnBeli1);
        SetupOpsiUI(option2, imgItem2, txtName2, txtPrice2, txtBonus2, btnBeli2);
        SetupOpsiUI(option3, imgItem3, txtName3, txtPrice3, txtBonus3, btnBeli3);

        popupPanel.SetActive(true);
    }

    void SetupOpsiUI(ItemOption option, Image img, TextMeshProUGUI name, TextMeshProUGUI price, TextMeshProUGUI bonus, Button btn)
    {
        if (img != null && option.itemSprite != null)
            img.sprite = option.itemSprite;

        if (name != null)
            name.text = option.itemName;

        if (price != null)
            price.text = $"Rp {option.price:N0}";

        if (bonus != null)
            bonus.text = $"+{option.ratingBonus} ⭐";
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

        if (selectedOption == null) return;
        if (GameManager.Instance == null) return;

        if (GameManager.Instance.KurangiUang(selectedOption.price))
        {
            GameManager.Instance.TambahRating(selectedOption.ratingBonus);

            // Simpan nama sprite yang dipilih
            string spriteName = selectedOption.itemSprite != null ? selectedOption.itemSprite.name : "";
            SavePurchaseState(spriteName);

            // Update sprite langsung
            if (selectedOption.itemSprite != null)
                spriteRenderer.sprite = selectedOption.itemSprite;

            isPurchased = true;
            if (boxCollider != null)
                boxCollider.enabled = false;
            if (interactHint != null)
                interactHint.SetActive(false);
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
        string spriteName = "";

        switch (slotID)
        {
            case "Dalam_1":
                purchased = GameManager.Instance.slotDalam1_Dibeli;
                spriteName = GameManager.Instance.slotDalam1_SpriteName;
                break;
            case "Dalam_2":
                purchased = GameManager.Instance.slotDalam2_Dibeli;
                spriteName = GameManager.Instance.slotDalam2_SpriteName;
                break;
            case "Dalam_3":
                purchased = GameManager.Instance.slotDalam3_Dibeli;
                spriteName = GameManager.Instance.slotDalam3_SpriteName;
                break;
            case "Dalam_4":
                purchased = GameManager.Instance.slotDalam4_Dibeli;
                spriteName = GameManager.Instance.slotDalam4_SpriteName;
                break;
            case "Dalam_5":
                purchased = GameManager.Instance.slotDalam5_Dibeli;
                spriteName = GameManager.Instance.slotDalam5_SpriteName;
                break;
        }

        if (purchased)
        {
            // Cari sprite berdasarkan nama
            Sprite savedSprite = FindSpriteByName(spriteName);

            if (savedSprite != null)
                spriteRenderer.sprite = savedSprite;

            isPurchased = true;
            if (boxCollider != null)
                boxCollider.enabled = false;
            if (interactHint != null)
                interactHint.SetActive(false);
        }
    }

    void SavePurchaseState(string spriteName)
    {
        if (GameManager.Instance == null) return;

        switch (slotID)
        {
            case "Dalam_1":
                GameManager.Instance.slotDalam1_Dibeli = true;
                GameManager.Instance.slotDalam1_SpriteName = spriteName;
                break;
            case "Dalam_2":
                GameManager.Instance.slotDalam2_Dibeli = true;
                GameManager.Instance.slotDalam2_SpriteName = spriteName;
                break;
            case "Dalam_3":
                GameManager.Instance.slotDalam3_Dibeli = true;
                GameManager.Instance.slotDalam3_SpriteName = spriteName;
                break;
            case "Dalam_4":
                GameManager.Instance.slotDalam4_Dibeli = true;
                GameManager.Instance.slotDalam4_SpriteName = spriteName;
                break;
            case "Dalam_5":
                GameManager.Instance.slotDalam5_Dibeli = true;
                GameManager.Instance.slotDalam5_SpriteName = spriteName;
                break;
        }
    }

    Sprite FindSpriteByName(string spriteName)
    {
        if (string.IsNullOrEmpty(spriteName)) return null;

        // Cari di array allSprites
        if (allSprites != null)
        {
            foreach (Sprite sprite in allSprites)
            {
                if (sprite != null && sprite.name == spriteName)
                {
                    return sprite;
                }
            }
        }

        return null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}