using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TimerScript : MonoBehaviour
{
    public TMP_Text TimerCounter;
    public static float timeLeft;
    private Color initialColor;
    private Color transparentColor;

    public static bool isEnabled = false;

    void Start()
    {
        transparentColor = new Color(0, 0, 0, 0);
        initialColor = TimerCounter.color;

        if (MenuButtonScript.selectedGameMode == "Learning")
            SetTransparent();

        TimerCounter.text = "" + NumberWordsScript.timeInitialValue;
    }

    public static void SetEnabled()
    {
        isEnabled = true;

        switch (MenuButtonScript.selectedGameMode)
        {
            case "TimeAttack":
                timeLeft = NumberWordsScript.timeInitialValue;
                break;

            case "Learning":
                timeLeft = 0;
                break;
        }
        
    }

    public static void SetDisabled()
    {
        isEnabled = false;
    }

    public static void Reset()
    {
        SetDisabled();
        SetEnabled();
    }

    void Update()
    {
        if (isEnabled)
        {
            timeLeft -= Time.deltaTime;            

            if (MenuButtonScript.selectedGameMode == "TimeAttack")
            {
                if (timeLeft < 0 && timeLeft > -2)
                {
                    GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicScript>().DecreaseVolume();
                    SetTransparent();

                    TimeAttack.ClearTextWordToGuess();
                }
                    
                    

                if (timeLeft < -2)
                {
                    SceneManager.LoadScene("GameOverScene");
                    SetDisabled();
                }

                TimerCounter.text = "Time : " + Mathf.Round(timeLeft);
            }

            else if (MenuButtonScript.selectedGameMode == "Learning")
            {
                
            }
        }
    }

    void SetTransparent()
    {
        TimerCounter.color = transparentColor;
    }

    void ResetColor()
    {
        TimerCounter.color = initialColor;
    }
}
