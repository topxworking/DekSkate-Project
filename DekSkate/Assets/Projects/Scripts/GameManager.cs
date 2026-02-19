using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Player Stats")]
    public int score = 0;
    public float timeRemaining = 300f;
    public bool isGameActive = true;

    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
    public GameObject winPanel;
    public TextMeshProUGUI finalScoreText;

    void Update()
    {
        if (isGameActive)
        {
            if (timeRemaining > 0)
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
        Time.timeScale = 0f;
        isGameActive = false;

        int finalTimeLeft = Mathf.FloorToInt(timeRemaining);
        int timeBonus = finalTimeLeft * 50;
        int totalFinalScore = score + timeBonus;

        winPanel.SetActive(true);
        finalScoreText.text =
            $"SCORE: {score.ToString("D7")}\n" +
            $"TIME BONUS: ({finalTimeLeft}s x 50): {timeBonus.ToString("D7")}\n" +
            $"TOTAL: {totalFinalScore.ToString("D7")}";
    }
}