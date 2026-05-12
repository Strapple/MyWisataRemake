using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class IndeksManager : MonoBehaviour
{
    [Header("UI Panel")]
    public GameObject indeksPanel;
    public TextMeshProUGUI txtTitle;
    public Image imgItem;
    public TextMeshProUGUI txtAsal;
    public TextMeshProUGUI txtTahun;
    public TextMeshProUGUI txtPembuat;
    public TextMeshProUGUI txtKategori;
    public TextMeshProUGUI txtDeskripsi;
    public TextMeshProUGUI txtPageInfo;

    [Header("Locked Display")]
    public Sprite lockedSprite;
    public string lockedName = "???";
    public string lockedInfo = "Beli item ini untuk membuka informasi selengkapnya!";

    [Header("Buttons")]
    public Button btnPrev;
    public Button btnNext;
    public Button btnTutup;

    [Header("Progress")]
    public TextMeshProUGUI txtProgress;

    [Header("Data")]
    public List<IndeksItemData> allItems = new List<IndeksItemData>();

    private int currentIndex = 0;

    void Start()
    {
        if (indeksPanel != null) indeksPanel.SetActive(false);
        if (btnPrev != null) btnPrev.onClick.AddListener(PrevItem);
        if (btnNext != null) btnNext.onClick.AddListener(NextItem);
        if (btnTutup != null) btnTutup.onClick.AddListener(CloseIndeks);
        allItems.Sort((a, b) =>
        {
            int slotCompare = a.slotID.CompareTo(b.slotID);
            if (slotCompare != 0) return slotCompare;
            return a.opsiIndex.CompareTo(b.opsiIndex);
        });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.I))
            ToggleIndeks();
    }

    public void ToggleIndeks()
    {
        if (indeksPanel.activeSelf)
            CloseIndeks();
        else
            OpenIndeks();
    }

    void OpenIndeks()
    {
        indeksPanel.SetActive(true);
        currentIndex = 0;
        DisplayCurrentItem();
    }

    void CloseIndeks()
    {
        indeksPanel.SetActive(false);
    }

    void DisplayCurrentItem()
    {
        if (allItems.Count == 0) return;

        IndeksItemData item = allItems[currentIndex];
        bool isUnlocked = IsItemUnlocked(item);

        if (txtPageInfo != null)
            txtPageInfo.text = $"{currentIndex + 1} / {allItems.Count}";

        if (isUnlocked)
        {
            if (txtTitle != null) txtTitle.text = item.itemName;
            if (imgItem != null) imgItem.sprite = item.itemSprite;
            if (txtAsal != null) txtAsal.text = string.IsNullOrEmpty(item.asalDaerah) ? "" : $"📍 Asal: {item.asalDaerah}";
            if (txtTahun != null) txtTahun.text = string.IsNullOrEmpty(item.tahunDibuat) ? "" : $"📅 Tahun: {item.tahunDibuat}";
            if (txtPembuat != null) txtPembuat.text = string.IsNullOrEmpty(item.pembuat) ? "" : $"🎨 Pembuat: {item.pembuat}";
            if (txtKategori != null) txtKategori.text = $"📦 Kategori: {item.kategori}";
            if (txtDeskripsi != null) txtDeskripsi.text = item.deskripsi;
        }
        else
        {
            if (txtTitle != null) txtTitle.text = lockedName;
            if (imgItem != null) imgItem.sprite = lockedSprite;
            if (txtAsal != null) txtAsal.text = "";
            if (txtTahun != null) txtTahun.text = "";
            if (txtPembuat != null) txtPembuat.text = "";
            if (txtKategori != null) txtKategori.text = "";
            if (txtDeskripsi != null) txtDeskripsi.text = lockedInfo;
        }

        if (txtProgress != null)
            txtProgress.text = $"{GetUnlockedSlotCount()} / {GetTotalSlots()} Koleksi Terbuka";

        if (btnPrev != null) btnPrev.interactable = currentIndex > 0;
        if (btnNext != null) btnNext.interactable = currentIndex < allItems.Count - 1;
    }

    void NextItem() { if (currentIndex < allItems.Count - 1) { currentIndex++; DisplayCurrentItem(); } }
    void PrevItem() { if (currentIndex > 0) { currentIndex--; DisplayCurrentItem(); } }

    bool IsItemUnlocked(IndeksItemData item)
    {
        if (GameManager.Instance == null) return false;

        bool slotDibeli = IsSlotDibeli(item.slotID);
        if (!slotDibeli) return false;

        string savedName = GetItemName(item.slotID);
        return savedName == item.itemName;
    }

    bool IsSlotDibeli(string slotID)
    {
        switch (slotID)
        {
            case "Luar_A": return GameManager.Instance.slotLuarA_Dibeli;
            case "Luar_B": return GameManager.Instance.slotLuarB_Dibeli;
            case "Dalam_1": return GameManager.Instance.slotDalam1_Dibeli;
            case "Dalam_2": return GameManager.Instance.slotDalam2_Dibeli;
            case "Dalam_3": return GameManager.Instance.slotDalam3_Dibeli;
            case "Dalam_4": return GameManager.Instance.slotDalam4_Dibeli;
            case "Dalam_5": return GameManager.Instance.slotDalam5_Dibeli;
            case "Dalam_6": return GameManager.Instance.slotDalam6_Dibeli;
            case "Dalam_7": return GameManager.Instance.slotDalam7_Dibeli;
            case "Dalam_8": return GameManager.Instance.slotDalam8_Dibeli;
            case "Miniatur_1": return GameManager.Instance.slotMiniatur1_Dibeli;
            case "Lukisan_1": return GameManager.Instance.slotLukisan1_Dibeli;
            case "Lukisan_2": return GameManager.Instance.slotLukisan2_Dibeli;
            case "Lukisan_3": return GameManager.Instance.slotLukisan3_Dibeli;
            case "Lukisan_4": return GameManager.Instance.slotLukisan4_Dibeli;
            case "Lukisan_5": return GameManager.Instance.slotLukisan5_Dibeli;
            case "Lukisan_6": return GameManager.Instance.slotLukisan6_Dibeli;
        }
        return false;
    }

    string GetItemName(string slotID)
    {
        if (GameManager.Instance == null) return "";
        switch (slotID)
        {
            case "Luar_A": return GameManager.Instance.slotLuarA_ItemName;
            case "Luar_B": return GameManager.Instance.slotLuarB_ItemName;
            case "Dalam_1": return GameManager.Instance.slotDalam1_ItemName;
            case "Dalam_2": return GameManager.Instance.slotDalam2_ItemName;
            case "Dalam_3": return GameManager.Instance.slotDalam3_ItemName;
            case "Dalam_4": return GameManager.Instance.slotDalam4_ItemName;
            case "Dalam_5": return GameManager.Instance.slotDalam5_ItemName;
            case "Dalam_6": return GameManager.Instance.slotDalam6_ItemName;
            case "Dalam_7": return GameManager.Instance.slotDalam7_ItemName;
            case "Dalam_8": return GameManager.Instance.slotDalam8_ItemName;
            case "Miniatur_1": return GameManager.Instance.slotMiniatur1_ItemName;
            case "Lukisan_1": return GameManager.Instance.slotLukisan1_ItemName;
            case "Lukisan_2": return GameManager.Instance.slotLukisan2_ItemName;
            case "Lukisan_3": return GameManager.Instance.slotLukisan3_ItemName;
            case "Lukisan_4": return GameManager.Instance.slotLukisan4_ItemName;
            case "Lukisan_5": return GameManager.Instance.slotLukisan5_ItemName;
            case "Lukisan_6": return GameManager.Instance.slotLukisan6_ItemName;
        }
        return "";
    }

    // ⬇️ BARU: Hitung total slot unik
    int GetTotalSlots()
    {
        List<string> uniqueIDs = new List<string>();
        foreach (IndeksItemData item in allItems)
        {
            if (!uniqueIDs.Contains(item.slotID))
                uniqueIDs.Add(item.slotID);
        }
        return uniqueIDs.Count;
    }

    // ⬇️ BARU: Hitung slot yang sudah dibeli
    int GetUnlockedSlotCount()
    {
        List<string> uniqueIDs = new List<string>();
        foreach (IndeksItemData item in allItems)
        {
            if (!uniqueIDs.Contains(item.slotID))
                uniqueIDs.Add(item.slotID);
        }

        int count = 0;
        foreach (string slotID in uniqueIDs)
        {
            if (IsSlotDibeli(slotID))
                count++;
        }
        return count;
    }
}