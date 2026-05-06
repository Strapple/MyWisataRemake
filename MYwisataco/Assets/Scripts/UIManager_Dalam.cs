using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEditor.ShortcutManagement;

public class UIManager_Dalam : MonoBehaviour
{
    [Header("UI Text")]
    public TextMeshProUGUI txtUang;
    public TextMeshProUGUI txtRating;
    public Slider sliderKebersihan;

    [Header("Tombol UI (Opsional)")]
    public Button btnKerahkan;
    public Button btnToilet;
    public Button btnWarung;
    public Button btnTongSampah;
    public Button btnKeluarGedung;

    [Header("Bottom Bar Panel (untuk disembunyikan)")]
    public GameObject bottomBarPanel;

    [Header("Warning Settings")]
    public GameObject warningPanel;           // Panel peringatan
    public TextMeshProUGUI txtWarning;        // Teks peringatan
    public float warningThreshold = 50f;      // Muncul di bawah 50%
    public float criticalThreshold = 30f;

    [Header("Slider Colors")]
    public Image sliderFill;
    public Color normalColor = Color.green;
    public Color warningColor = Color.yellow;
    public Color criticalColor = Color.red; 

    private bool isInitialized = false;
    private bool isWarningActive = false;

    IEnumerator Start()
    {
        yield return new WaitUntil(() => GameManager.Instance != null);

        GameManager.Instance.OnDataUpdated += UpdateUI;

        if (btnKerahkan != null)
            btnKerahkan.onClick.AddListener(OnKerahkanClicked);
        if (btnToilet != null)
            btnToilet.onClick.AddListener(OnToiletClicked);
        if (btnWarung != null)
            btnWarung.onClick.AddListener(OnWarungClicked);
        if (btnTongSampah != null)
            btnTongSampah.onClick.AddListener(OnTongSampahClicked);
        if (btnKeluarGedung != null)
            btnKeluarGedung.onClick.AddListener(OnKeluarGedungClicked);

        // Sembunyikan bottom bar
        if (bottomBarPanel != null)
            bottomBarPanel.SetActive(false);

        isInitialized = true;

        UpdateUI();
        UpdateButtonStates();
    }

    void Update()
    {
        // SHORTCUT KEYBOARD
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Z))
        {
            OnKerahkanClicked();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.T))
        {
            OnToiletClicked();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.R))
        {
            OnWarungClicked();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.G))
        {
            OnTongSampahClicked();
        }
    }

    void OnDestroy()
    {
        if (isInitialized && GameManager.Instance != null)
        {
            GameManager.Instance.OnDataUpdated -= UpdateUI;
        }
    }

    void UpdateUI()
    {
        if (GameManager.Instance == null) return;

        txtUang.text = $"Rp {GameManager.Instance.uang:N0}";
        txtRating.text = $"{GameManager.Instance.rating:F1}";
        sliderKebersihan.value = GameManager.Instance.kebersihan / 100f;

        UpdateButtonStates();
        UpdateWarningDisplay();
    }

    void UpdateWarningDisplay()
    {
        if (warningPanel == null) return;

        float kebersihan = GameManager.Instance.kebersihan;

        if (sliderFill != null)
        {
            if (kebersihan <= criticalThreshold)
                sliderFill.color = criticalColor;
            else if (kebersihan <= warningThreshold)
                sliderFill.color = warningColor;
            else
                sliderFill.color = normalColor;
        }

        if (kebersihan <= warningThreshold)
        {
            if (!warningPanel.activeSelf)
                warningPanel.SetActive(true);

            // Update teks dan warna
            if (txtWarning != null)
            {
                if (kebersihan <= criticalThreshold)
                {
                    txtWarning.text = "⚠️ KRITIS! Kebersihan Sangat Rendah! ⚠️\nTekan [1] atau [K] untuk KERAHKAN PETUGAS!";
                    txtWarning.color = Color.red;
                }
                else
                {
                    txtWarning.text = "⚠️ Kebersihan Rendah!\nTekan [1] atau [K] untuk KERAHKAN PETUGAS!";
                    txtWarning.color = Color.yellow;
                }
            }

            // Efek berkedip jika kritis
            if (kebersihan <= criticalThreshold && !isWarningActive)
            {
                isWarningActive = true;
                StartCoroutine(BlinkWarning());
            }
        }
        else
        {
            if (warningPanel.activeSelf)
            {
                warningPanel.SetActive(false);
                isWarningActive = false;
                StopAllCoroutines();
            }
        }
    }

    IEnumerator BlinkWarning()
    {
        float blinkSpeed = 0.5f;

        while (GameManager.Instance != null && GameManager.Instance.kebersihan <= criticalThreshold)
        {
            if (warningPanel != null)
                warningPanel.SetActive(!warningPanel.activeSelf);
            yield return new WaitForSeconds(blinkSpeed);
        }

        // Pastikan panel aktif saat selesai (kalau masih di bawah threshold)
        if (warningPanel != null && GameManager.Instance != null)
            warningPanel.SetActive(GameManager.Instance.kebersihan <= warningThreshold);

        isWarningActive = false;
    }
    void UpdateButtonStates()
    {
        if (GameManager.Instance == null) return;

        if (btnToilet != null)
            btnToilet.interactable = !GameManager.Instance.toiletDibeli;
        if (btnWarung != null)
            btnWarung.interactable = !GameManager.Instance.warungDibeli;
        if (btnTongSampah != null)
            btnTongSampah.interactable = !GameManager.Instance.tongSampahDibeli;
    }

    void OnKerahkanClicked()
    {
        GameManager.Instance?.KerahkanPetugas();
    }

    void OnToiletClicked()
    {
        GameManager.Instance?.BeliToilet();
    }

    void OnWarungClicked()
    {
        GameManager.Instance?.BeliWarung();
    }

    void OnTongSampahClicked()
    {
        GameManager.Instance?.BeliTongSampah();
    }

    void OnKeluarGedungClicked()
    {
        SceneManager.LoadScene("Scene_Luar");
    }
}