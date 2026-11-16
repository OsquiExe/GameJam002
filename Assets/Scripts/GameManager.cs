using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    private int score = 0;
    private int highScore = 0;

    public TextMeshProUGUI scoreText;
    private int playersTrapped = 0;
    void Start()
    {
        ShowText();
    }
    void Update()
    {
        
    }

    public void AddScore(int amount)
    {
        score += amount;
        if (score > highScore)
        {
            highScore = score;
        }
        ShowText();
    }

    public void ShowText()
    {
        scoreText.text = $"Score: {score}\nHigh Score: {highScore}";
    }

    public void PlayerTrapped()
    {
        playersTrapped++;

        if (playersTrapped == 1)
        {
            Debug.Log("atrapado");
        }

        if (playersTrapped >= 2)
        {
            GameOver();
        }

    }

    public void PlayerFreed()
    {
        playersTrapped--;
        if (playersTrapped < 0)
        {
            playersTrapped = 0;
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over! Final Score: " + score);
    }
}
