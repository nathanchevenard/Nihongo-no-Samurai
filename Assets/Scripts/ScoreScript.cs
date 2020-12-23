using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    public TMP_Text Counter;
    public static int counterValue = 0;
    private float initialFontSize;

    private Color initialColor;
    private Color transparentColor;

    void Start()
    {
        transparentColor = new Color(0, 0, 0, 0);
        initialColor = new Color(1.0f, 1.0f, 0.0f);

        Counter.color = initialColor;

        initialFontSize = Counter.fontSize;

        if (MenuButtonScript.selectedGameMode == "Learning" && SceneManager.GetActiveScene().name == "TimeAttackScene")
            SetTransparent();

        Counter = GetComponent<TMP_Text>();
    }

    void Update()
    {
        Counter.text = "Score : " + counterValue;

        if (SceneManager.GetActiveScene().name == "GameOverScene" && Counter.color == transparentColor)
            ResetColor();
    }

    void SetTransparent()
    {
        Counter.color = transparentColor;
    }

    void ResetColor()
    {
        Counter.color = initialColor;        
    }

    public void IncrementScore()
    {
        counterValue += 1;
        if (counterValue <= 10)
        {
            Counter.fontSize += 1;

            Counter.color = new Color(1.0f, Counter.color.g - 0.1f, 0.0f);
        }
    }

    public void ResetScore()
    {
        Counter.fontSize = initialFontSize;
        Counter.color = initialColor;
        counterValue = 0;       
    }
}
