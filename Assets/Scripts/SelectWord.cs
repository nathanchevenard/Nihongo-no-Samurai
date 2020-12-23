using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;
using TMPro;

public class SelectWord : MonoBehaviour
{
    private string myWord;
    private string myRomaji;
    private string myTranslation;
    public bool correctWord = false;
    public Texture2D cursorAttack;
    public Texture2D cursorBase;
    public GameObject Wrong;
    public GameObject Slash;

    private GameObject slashImage;

    private Animator anim;

    private bool isActive = true;


    //stores the word in attribute myWord and loads the new sprite to display
    public void ReceiveWord(string newWord, string newRomaji, string newTranslation)
    {
        myWord = newWord;
        myTranslation = newTranslation;
        myRomaji = newRomaji;

        if (MenuButtonScript.typeOfWords == "Vocabulary")
        {
            GetComponent<TMP_Text>().rectTransform.sizeDelta = new Vector2(300, 65);
            GetComponent<TMP_Text>().fontSize = 50;
        }

        GetComponent<TMP_Text>().text = myWord;        
    }

    public void ReceiveWord(string newWord, string newRomaji)
    {
        myWord = newWord;
        myRomaji = newRomaji;

        if (MenuButtonScript.typeOfWords == "Vocabulary")
        {
            GetComponent<Image>().rectTransform.sizeDelta = new Vector2(300, 65);
        }

        GetComponent<TMP_Text>().text = myWord;
    }


    //set this word as the "correct word", i.e. the word the player is supposed to choose
    public void SetCorrectWord()
    {
        correctWord = true;
    }

    public void SetInactive()
    {
        isActive = false;
    }

    public bool IsActive()
    {
        return isActive;
    }

    public string GetWord()
    {
        return myWord;
    }

    public string GetTranslation()
    {
        return myTranslation;
    }

    public string GetRomaji()
    {
        return myRomaji;
    }

    //when clicked on, the word will check whether he is the correct one or not
    public void CheckWord()
    {
        //if correct, add 1 point to the score and update the highscore
        if (correctWord && isActive)
        {
            CharacterAnimation.attackAnimation();

            //ask the main script StartGame to pick new words
            TimeAttack.needRefresh = true;

            CorrectWordsCounterScript.correctWordsValue += 1;

            if (MenuButtonScript.selectedGameMode == "TimeAttack")
            {
                GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreScript>().IncrementScore();

                if (ScoreScript.counterValue > HighscoreScript.highscoreValue)
                {
                    HighscoreScript.highscoreValue = ScoreScript.counterValue;
                }                
            }

            SoundManagerScript.PlaySoundClic("correct");

            if (MenuButtonScript.selectedGameMode == "Learning")
            {
                TimerScript.timeLeft = 1.5f;
            }

            SoundManagerScript.PlaySoundWord(myRomaji);

            GameObject slashImage = Instantiate(Slash, new Vector3(0, 0, 0), Quaternion.identity);
            slashImage.transform.SetParent(GetComponent<TMP_Text>().transform, false);

            TimeAttack.timerWhenClicked = TimerScript.timeLeft;
        }

        //if incorrect, put an "incorrect" image in front of the word and resets the score value
        else if (!correctWord && isActive)
        {
            if (MenuButtonScript.selectedGameMode == "TimeAttack")
            {
                GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreScript>().ResetScore();
            }

            GameObject wrongImage = Instantiate(Wrong, new Vector3(0, 0, 0), Quaternion.identity);
            wrongImage.transform.SetParent(GetComponent<TMP_Text>().transform, false);

            SoundManagerScript.PlaySoundClic("wrong");

            CorrectWordsCounterScript.wrongWordsValue += 1;
        }

        UnityEngine.Cursor.SetCursor(cursorBase, Vector2.zero, CursorMode.ForceSoftware);
        SetInactive();
    }

    public void OnMouseEnter()
    {
        if (isActive)
            UnityEngine.Cursor.SetCursor(cursorAttack, Vector2.zero, CursorMode.ForceSoftware);
    }

    public void OnMouseExit()
    {
        UnityEngine.Cursor.SetCursor(cursorBase, Vector2.zero, CursorMode.ForceSoftware);
    }

    public void RemoveSlash()
    {
        Color oldColor = Slash.GetComponent<Image>().color;
        Slash.GetComponent<Image>().color = new Color(oldColor.r, oldColor.g, oldColor.b, oldColor.a - 0.001f);
        Debug.Log("yo");
    }

    public void ResetTransparency()
    {
        Color oldColor = Slash.GetComponent<Image>().color;
        Slash.GetComponent<Image>().color = new Color(oldColor.r, oldColor.g, oldColor.b, 1f);
        Debug.Log("yo");
    }
}