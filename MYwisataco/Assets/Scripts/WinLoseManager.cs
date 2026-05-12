using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class WinLoseManager : MonoBehaviour
{
    [Header("Win Settings")]
    public float winTimerDuration = 10f;
    private float winTimer = 0f;
    private bool isWinning = false;

    [Header("UI Popup")]
    public GameObject winPopup;
    public GameObject losePopup;
    public TextMeshProUGUI txtWinMessage;
    public TextMeshProUGUI txtLoseMessage;

    [Header("UI Timer")]                           // <-- TAMBAHKAN INI
    public GameObject timerPanel;
    public TextMeshProUGUI txtTimerValue;

    [Header("Buttons")]
    public Button btnMainMenu_Win;
    public Button btnMainMenu_Lose;
    public Button btnRetry_Lose;
    public Button btnMainLagi_Win;

    void Start()
    {
        if (winPopup != null) winPopup.SetActive(false);
        if (losePopup != null) losePopup.SetActive(false);
        if (timerPanel != null) timerPanel.SetActive(false);  // <-- TAMBAHKAN

        if (btnMainMenu_Win != null)
            btnMainMenu_Win.onClick.AddListener(GoToMainMenu);
        if (btnMainMenu_Lose != null)
            btnMainMenu_Lose.onClick.AddListener(GoToMainMenu);
        if (btnRetry_Lose != null)
            btnRetry_Lose.onClick.AddListener(RetryGame);
        if (btnMainLagi_Win != null)
            btnMainLagi_Win.onClick.AddListener(RetryGame);
    }

    void Update()
    {
        if (GameManager.Instance == null) return;

        CheckLoseCondition();
        CheckWinCondition();
    }

    void CheckLoseCondition()
    {
        if (GameManager.Instance.uang <= 0)
        {
            if (losePopup != null && !losePopup.activeSelf)
            {
                losePopup.SetActive(true);
                if (timerPanel != null) timerPanel.SetActive(false);  // <-- SEMBUNYIKAN TIMER
                Time.timeScale = 0f;
            }
        }
    }

    void CheckWinCondition()
    {
        if (GameManager.Instance.rating >= 5.0f)
        {
            if (!isWinning)
            {
                isWinning = true;
                winTimer = winTimerDuration;

                // TAMPILKAN TIMER
                if (timerPanel != null)
                {
                    timerPanel.SetActive(true);
                }
            }

            winTimer -= Time.deltaTime;

            // UPDATE UI TIMER
            if (txtTimerValue != null)
            {
                int secondsLeft = Mathf.CeilToInt(winTimer);
                txtTimerValue.text = secondsLeft.ToString();

                // Ubah warna kalau hampir habis
                if (secondsLeft <= 3)
                    txtTimerValue.color = Color.red;
                else
                    txtTimerValue.color = Color.yellow;
            }

            if (winTimer <= 0f)
            {
                if (winPopup != null && !winPopup.activeSelf)
                {
                    winPopup.SetActive(true);
                    if (timerPanel != null) timerPanel.SetActive(false);  // <-- SEMBUNYIKAN TIMER
                    Time.timeScale = 0f;
                }
            }
        }
        else
        {
            // Rating turun di bawah 3.0
            if (isWinning)
            {
                isWinning = false;
                winTimer = winTimerDuration;

                // SEMBUNYIKAN TIMER
                if (timerPanel != null)
                {
                    timerPanel.SetActive(false);
                }
            }
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        if (GameManager.Instance != null)
            Destroy(GameManager.Instance.gameObject);
        SceneManager.LoadScene("MainMenu");
    }

    public void RetryGame()
    {
        Time.timeScale = 1f;
        if (GameManager.Instance != null)
            Destroy(GameManager.Instance.gameObject);
        SceneManager.LoadScene("Scene_Luar");
    }
}