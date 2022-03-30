using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text highScoreText;

    private bool gameStarted;
    private int score;

    public void StartGame()
    {
        gameStarted = true;
        FindObjectOfType<RoadScript>().StartBuilding();
    }

    public bool IsGameStarted()
    {
        return gameStarted;
    }

    public void EndGame()
    {
        SceneManager.LoadScene(0);
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();

        if(score > GetHighScore())
        {
            PlayerPrefs.SetInt("HighScore", score);
            highScoreText.text = "Best: " + score.ToString();
        }
    }

    public int GetScore()
    {
        return score;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartGame();
        }
    }

    private void Awake()
    {
        highScoreText.text = "Best: " + GetHighScore().ToString();
    }

    private int GetHighScore()
    {
        int highScore = PlayerPrefs.GetInt("HighScore");
        return highScore;
    }
}
