using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumberWordsScript : MonoBehaviour
{
    public Slider NbWordsSlider;
    public Slider NbWordsToLearnSlider;
    public Slider SelectTimeSlider;
    public TMP_Text NbWordsText;
    public TMP_Text NbWordsToLearnText;
    public TMP_Text SelectTimeText;

    public static int numberOfRandomWords = 6;
    public static int numberWordsToLearn = 5;
    public static int timeInitialValue = 30;


    // Update is called once per frame
    void Update()
    {
        numberOfRandomWords = Convert.ToInt32(NbWordsSlider.value);
        NbWordsText.text = "Random words : " + numberOfRandomWords;

        timeInitialValue = Convert.ToInt32(SelectTimeSlider.value) * 15;
        SelectTimeText.text = "Game duration : " + timeInitialValue + " secs";

        numberWordsToLearn = Convert.ToInt32(NbWordsToLearnSlider.value);
        NbWordsToLearnText.text = "Words to learn : " + numberWordsToLearn;
    }
}
