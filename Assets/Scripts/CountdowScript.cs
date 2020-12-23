using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CountdowScript : MonoBehaviour
{
    public GameObject Character;
    public TMP_Text CountdownText;
    float timeLeft = 5f;
    float goDuration = 1f;


    void Start()
    {
        Time.timeScale = 1;

        //instantiate the character in the middle of the screen
        /*
        GameObject samurai = Instantiate(Character, new Vector3(0, 50, 0), Quaternion.identity);
        samurai.transform.SetParent(GameObject.Find("Canvas").transform, false);
        */
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;

        //when the text has a different value from actual time, meaning a new number will be displayed
        bool isTextNew = (CountdownText.text != Mathf.Ceil(timeLeft - goDuration).ToString());

        //plays drum sound every time number changes
        if (isTextNew && timeLeft - goDuration > 0 && Mathf.Ceil(timeLeft - goDuration) < 4 && CountdownText.text != "GO")
        {
            SoundManagerScript.PlaySoundDrum();
        }

        //shows countdown from 3 to 1
        if (timeLeft - goDuration >= 0 && Mathf.Ceil(timeLeft - goDuration) < 4)
        {
            CountdownText.text = "" + Mathf.Ceil(timeLeft - goDuration);
        }

        //changes text to GO
        else if (timeLeft < goDuration && timeLeft >= 0)
        {
            //plays loud drum sound on GO appearance
            if (CountdownText.text != "GO")
            {
                SoundManagerScript.PlaySoundDrumLoud();
            }

            CountdownText.text = "GO";
        }    

        //loads game screen after GO
        else if (timeLeft < 0)
        {
            SceneManager.LoadScene("TimeAttackScene");
        }    
    }
}
