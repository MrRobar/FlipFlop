using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverWindow : MonoBehaviour
{
    public Text scoreText;
    public Text highScore;


    private void Start() 
    {
        PlayerController.instance.OnDied += Player_OnDied;
        Hide();
    }

    private void Player_OnDied(object sender, EventArgs e)
    {
        scoreText.text = "Score\n" + Level.instance.GetScore().ToString();
        ScoreRecord.SetHightScore(PlayerPrefs.GetInt("highscore"), Level.instance.GetScore());
        highScore.text = "Highscore\n" + PlayerPrefs.GetInt("highscore").ToString();
        ScoreRecord.SetCrystalls(Level.instance.crystallCount);
        Show();
    }

    public void Reload() 
    {
        SceneManager.LoadScene("Main");
    }
    public void ToMenu() 
    {
        SceneManager.LoadScene("Menu");
    }

    private void Hide() 
    {
        gameObject.SetActive(false);
    }
    private void Show() 
    {
        gameObject.SetActive(true);
    }

}
