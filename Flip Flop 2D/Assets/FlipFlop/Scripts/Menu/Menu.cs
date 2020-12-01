using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject optionsPanel;
    public GameObject marketPanel;

    public Text highScoreText;
    public Text crystallScoreText;

    private void Awake() 
    {
        HideOptions();
        OpenMainPanel();
        highScoreText.text = "HighScore\n" + PlayerPrefs.GetInt("highscore").ToString();
        crystallScoreText.text = PlayerPrefs.GetInt("Crystalls").ToString();
    }

    public void StartGame() 
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }

    public void ExitGame() 
    {
        Application.Quit();
    }

    public void OpenOptions() 
    {
        optionsPanel.SetActive(true);
    }

    public void HideOptions() 
    {
        optionsPanel.SetActive(false);
    }

    public void OpenMainPanel() 
    {
        mainPanel.SetActive(true);
    }

    public void HideMainPanel() 
    {
        mainPanel.SetActive(false);
    }

    public void OpenMarket() 
    {
        marketPanel.SetActive(true);
    }

    public void CloseMarket() 
    {
        marketPanel.SetActive(false);
    }
}
