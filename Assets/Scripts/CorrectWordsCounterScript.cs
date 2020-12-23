using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CorrectWordsCounterScript : MonoBehaviour
{
    public TMP_Text CorrectWordsCounter;
    public static int correctWordsValue = 0;
    public static int wrongWordsValue = 0;

    void Start()
    {
        if (CorrectWordsCounter.name == "CorrectWordsCounter")
            CorrectWordsCounter.text = "Correct words : " + correctWordsValue;
        else if (CorrectWordsCounter.name == "WrongWordsCounter")
            CorrectWordsCounter.text = "Mistakes : " + wrongWordsValue;
    }
}
