using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShortcutUIManager : MonoBehaviour
{
    [Header("Shortcut Items")]
    public ShortcutItemUI itemKerahkan;
    public ShortcutItemUI itemToilet;
    public ShortcutItemUI itemWarung;
    public ShortcutItemUI itemTongSampah;

    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnDataUpdated += UpdateAllItems;
        }

        itemKerahkan.Setup("Kerahkan", "1/Z", 5000);
        itemToilet.Setup("Toilet", "2/T", 25000);
        itemWarung.Setup("Warung", "3/R", 35000);
        itemTongSampah.Setup("Tong Sampah", "4/G", 50000);

        UpdateAllItems();
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnDataUpdated -= UpdateAllItems;
        }
    }

    // ⬇️ UBAH INI JADI PUBLIC ⬇️
    public void UpdateAllItems()
    {
        if (GameManager.Instance == null) return;

        itemToilet.SetPurchased(GameManager.Instance.toiletDibeli);
        itemWarung.SetPurchased(GameManager.Instance.warungDibeli);
        itemTongSampah.SetPurchased(GameManager.Instance.tongSampahDibeli);
        itemKerahkan.SetPurchased(false);
    }
}

[System.Serializable]
public class ShortcutItemUI
{
    public Image iconImage;
    public TextMeshProUGUI txtName;
    public TextMeshProUGUI txtKey;
    public TextMeshProUGUI txtPrice;
    public Image statusImage;
    public Color purchasedColor = Color.green;
    public Color notPurchasedColor = Color.gray;

    private bool isPurchased = false;

    public void Setup(string name, string key, int price)
    {
        if (txtName != null) txtName.text = name;
        if (txtKey != null) txtKey.text = $"[{key}]";
        if (txtPrice != null) txtPrice.text = $"Rp {price:N0}";
    }

    public void SetPurchased(bool purchased)
    {
        isPurchased = purchased;

        if (statusImage != null)
        {
            statusImage.color = purchased ? purchasedColor : notPurchasedColor;
        }

        if (txtPrice != null)
        {
            if (purchased)
            {
                txtPrice.text = $"<s>Rp {GetPriceFromText():N0}</s>";
                txtPrice.color = Color.gray;
            }
            else
            {
                txtPrice.color = Color.white;
            }
        }
    }

    private int GetPriceFromText()
    {
        if (txtPrice == null) return 0;

        string text = txtPrice.text.Replace("Rp ", "").Replace(",", "").Replace("<s>", "").Replace("</s>", "");
        int.TryParse(text, out int price);
        return price;
    }
}