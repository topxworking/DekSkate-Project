using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public PlayerData playerData;

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
}
