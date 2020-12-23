using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HighscoreScript : MonoBehaviour
{
    public TMP_Text HighscoreCounter;
    public static int highscoreValue = 0;
    private Color initialColor;
    private Color transparentColor;

    void Start()
    {
        transparentColor = new Color(0, 0, 0, 0);
        initialColor = HighscoreCounter.color;

        if (MenuButtonScript.selectedGameMode == "Learning" && SceneManager.GetActiveScene().name == "TimeAttackScene")
            SetTransparent();

        HighscoreCounter = GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (MenuButtonScript.selectedGameMode == "TimeAttack")
            HighscoreCounter.text = "Highscore : " + highscoreValue;

        else if (MenuButtonScript.selectedGameMode == "Learning")
        {
            HighscoreCounter.text = "Number of words learnt : " + (NumberWordsScript.numberWordsToLearn) ;
            HighscoreCounter.fontSize = 40;
        }

        if (SceneManager.GetActiveScene().name == "GameOverScene" && HighscoreCounter.color == transparentColor)
            ResetColor();
    }

    void SetTransparent()
    {
        HighscoreCounter.color = transparentColor;
    }

    void ResetColor()
    {
        HighscoreCounter.color = initialColor;
    }
}
