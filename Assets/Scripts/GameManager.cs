using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    private int score = 0;
    private int highScore = 0;

    private int timer;
    private float timerInterval = 1.0f;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public GameObject panelTrapped;
    public GameObject panelGameOver;
    public TextMeshProUGUI scoreGameOverText;

    public AudioSource gameOverSound;
    public int Difficulty { get; private set; }
    public int EnemiesLife { get; private set; }
    public int PlayersTrapped { get; private set; } = 0;

    private void Awake()
    {
        if(PlayerPrefs.HasKey("highScore"))
        {
            highScore = PlayerPrefs.GetInt("highScore", 0);
        }
        //dificultad facil = 1;
        //dificultad normal = 2;
        //dificultad dificil = 3;
        Difficulty = PlayerPrefs.GetInt("difficulty", 2); // valor predeterminado es 2 (normal)
        switch (Difficulty)
        {
            case 1:
                timer = 120; // 2 minutos
                EnemiesLife = 1; // enemigos con 1 de vida
                break;
            case 2:
                timer = 90; // 1 minuto y 30 segundos
                EnemiesLife = 1;    // enemigos con 1 de vida
                break;
            case 3:
                timer = 60; // 1 minuto 
                EnemiesLife = 1; // enemigos con 1 de vida
                break;
            default:
                timer = 90;
                EnemiesLife = 1;
                break;
        }
    }
    void Start()
    {
        timerText.text = timer.ToString();
        ShowScore();
    }
    void Update()
    {
        timerInterval -= Time.deltaTime;
        if (timerInterval <= 0)
        {
            timer--;
            timerText.text = timer.ToString();
            timerInterval = 1.0f;
            if(timer <= 0)
            {
                GameOver();
            }
        }
    }
    public void AddTime()
    {
        switch (Difficulty)
        {
            case 1:
                timer += 60; //  agrega 1 minuto
                break;
            case 2:
                timer = 45; // agrega 45 segundos
                break;
            case 3:
                timer = 35; // agrega 35 segundos
                break;
        }
        timerText.text = timer.ToString();
    }

    public void AddTime(int seconds)
    {
        timer += seconds;
        timerText.text = timer.ToString();
    }   

    public void AddScore(int amount)
    {
        score += amount;
        if (score > highScore)
        {
            highScore = score;
        }
        if(score < 0)
        {
            score = 0;
        }
        ShowScore();
    }

    public void ShowScore()
    {
        scoreText.text = $"Score: {score}\nHigh Score: {highScore}";
        scoreGameOverText.text = $"Score: {score}\nHigh Score: {highScore}";
    }

    public void PlayerTrapped()
    {
        PlayersTrapped++;
        AddScore(-50);
        AddTime(-5);
        if (PlayersTrapped == 1)
        {
            panelGameOver.SetActive(false);
            Debug.Log("atrapado");  
            panelTrapped.SetActive(true);
        }

        if (PlayersTrapped >= 2)
        {
            GameOver();
        }

    }

    public void PlayerFreed()
    {
        PlayersTrapped--;
        if (PlayersTrapped < 0)
        {
            PlayersTrapped = 0;
        }
        if(PlayersTrapped == 0)
        {
            panelTrapped.SetActive(false);
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over! Final Score: " + score);
        PlayerPrefs.SetInt("highScore", highScore);
        PlayerPrefs.Save();
        if (gameOverSound) gameOverSound.Play();
        panelTrapped.SetActive(false);
        panelGameOver.SetActive(true);
        Time.timeScale = 0f;
    }
}
