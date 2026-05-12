using UnityEngine;

[System.Serializable]
public class IndeksItemData
{
    public string itemName;           // "Miniatur Patung Pancoran"
    public Sprite itemSprite;         // Gambar item
    public string asalDaerah;         // "Jakarta, DKI Jakarta"
    public string tahunDibuat;        // "1963"
    public string pembuat;            // "Edhi Sunarso"
    public string kategori;           // "Patung & Monumen"

    [TextArea(3, 6)]
    public string deskripsi;          // Deskripsi panjang

    public string slotID;             // "Miniatur_1" — untuk cek status beli
    public int opsiIndex;               // 1, 2, atau 3
}