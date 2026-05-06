using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Fungsi untuk tombol Play
    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene("Scene_Luar");
    }

    // Fungsi untuk tombol Quit
    public void OnQuitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}