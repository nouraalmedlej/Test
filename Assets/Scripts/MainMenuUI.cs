using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] string gameplaySceneName = "GameScene"; // set exact name
    [Header("Optional Panels")]
    [SerializeField] GameObject settingsPanel;

    void Awake()
    {
     
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Buttons
    public void OnPlayClicked()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void OnSettingsClicked()
    {
        if (settingsPanel) settingsPanel.SetActive(true);
    }

    public void OnCloseSettingsClicked()
    {
        if (settingsPanel) settingsPanel.SetActive(false);
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }
}

