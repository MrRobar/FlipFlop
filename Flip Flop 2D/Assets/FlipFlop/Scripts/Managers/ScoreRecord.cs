using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreRecord : MonoBehaviour
{
    public static int GetHightScore() 
    {
        return PlayerPrefs.GetInt("highscore");
    }

    public static void SetHightScore(int oldV, int newV)
    {
        if(newV > oldV) 
        {
            PlayerPrefs.SetInt("highscore", newV);
        }
        else 
        {
            PlayerPrefs.SetInt("highscore", oldV);
        }
        PlayerPrefs.Save();
    }

    public static void SetCrystalls(int value) 
    {
        int val = GetCrystalls();
        PlayerPrefs.SetInt("Crystalls", val + value);
        PlayerPrefs.Save();
    }

    public static int GetCrystalls() 
    {
        return PlayerPrefs.GetInt("Crystalls");
    }
}
