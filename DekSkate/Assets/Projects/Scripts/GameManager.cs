using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Player Stats")]
    public float health = 100f;
    public int score = 0;
    public float timeRemaining = 60f;
    public bool isGameActive = true;

    [Header("UI References")]
    public Slider healthSlider;
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
        healthSlider.value = health;
        scoreText.text = "Score: " + score;
        
        int displayTime = Mathf.FloorToInt(timeRemaining);
        timeText.text = "Time: " + displayTime.ToString();
    }

    public void AddScore(int amount)
    {
        score += amount;
    }

    public void EndLevel()
    {
        isGameActive = false;
        
        int finalTimeLeft = Mathf.FloorToInt(timeRemaining);
        int timeBonus = finalTimeLeft * 50;
        int totalFinalScore = score + timeBonus;

        winPanel.SetActive(true);
        finalScoreText.text =
            $"Score: {score}\n" +
            $"Time Bonus: ({finalTimeLeft}s x 50): {timeBonus}\n" +
            $"Total: {totalFinalScore}";
    }
}
