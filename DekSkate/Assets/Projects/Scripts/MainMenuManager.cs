using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public PlayerData playerData;

    public GameObject playButton;
    public GameObject quitButton;
    public GameObject settingsPanel;

    public void StartButton()
    {
        SceneManager.LoadScene("1-1");
        playerData.ResetData();
    }

    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }

    public void SettingsButton()
    {
        if (settingsPanel != null)
        {
            bool isActive = settingsPanel.activeSelf;
            settingsPanel.SetActive(!isActive);

            playButton.SetActive(isActive);
            quitButton.SetActive(isActive);
        }
    }
}
