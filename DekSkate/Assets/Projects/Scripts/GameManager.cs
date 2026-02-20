using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Player Data")]
    public PlayerData playerData;

    [Header("Player Stats")]
    public int score = 0;
    public float timeRemaining = 300f;
    public bool isGameActive = true;

    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
    public GameObject winPanel;
    public TextMeshProUGUI finalScoreText;
    public GameObject pausePanel;
    public GameObject pauseButton;

    private void Awake()
    {
        pauseButton.SetActive(true);

        if (playerData != null)
        {
            score = playerData.totalScore;
        }
        UpdateUI();
    }

    void Update()
    {
        if (isGameActive)
        {
            if (timeRemaining > 1)
            {
                timeRemaining -= Time.deltaTime;
                UpdateUI();
            }
            else
            {
                EndLevel();
            }
        }
    }

    void UpdateUI()
    {
        scoreText.text = "SCORE: " + score.ToString("D7");
        int displayTime = Mathf.FloorToInt(timeRemaining);
        timeText.text = "TIME: " + displayTime.ToString();
    }

    public void AddScore(int amount)
    {
        score += amount;
    }

    public void EndLevel()
    {
        if (!isGameActive) return;

        Time.timeScale = 0f;
        isGameActive = false;

        int finalTimeLeft = Mathf.FloorToInt(timeRemaining);
        int timeBonus = finalTimeLeft * 50;
        int totalFinalScore = score + timeBonus;

        if (playerData != null)
        {
            playerData.totalScore = totalFinalScore;
        }

        winPanel.SetActive(true);
        finalScoreText.text =
            $"SCORE: {score.ToString("D7")}\n" +
            $"TIME BONUS: ({finalTimeLeft}s x 50): {timeBonus.ToString("D7")}\n" +
            $"TOTAL: {totalFinalScore.ToString("D7")}";
    }

    public void NextLevelButton(string nextSceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nextSceneName);
    }

    public void RestartButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("1-1");
        playerData.ResetData();
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        playerData.ResetData();
    }

   public void PauseButton()
    {
        pausePanel.SetActive(true);
        pauseButton.SetActive(false);
        Time.timeScale = 0f;
    }

    public void ResumeButton()
    {
        pausePanel?.SetActive(false);
        pauseButton?.SetActive(true);
        Time.timeScale = 1f;
    }
}
