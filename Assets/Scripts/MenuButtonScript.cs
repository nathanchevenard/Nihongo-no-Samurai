using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtonScript : MonoBehaviour
{
    //type of words
    public static string typeOfWords = "";

    //name of game mode
    public static string selectedGameMode = "";

    //path of the text file containing words that will be guessed
    public static string pathOfWordsFile = "";

    void Start()
    {
        //set the cursor back to normal
        UnityEngine.Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);

        if (gameObject.name == "SoundButton" && MusicScript.isMuted)
            GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Buttons/SoundMuted");
    }

    public void OnClick()
    {
        string nameOfClickedButton = EventSystem.current.currentSelectedGameObject.name;

        switch(nameOfClickedButton)
        {
            case "MenuButton":
                GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicScript>().PlayMenuMusic();
                goto case "GoBackButton";

            case "GoBackButton":
                SceneManager.LoadScene("MenuScene");
                break;

            case "SoundButton":
                if (MusicScript.isMuted == false)
                {
                    GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicScript>().MuteMusic(true);
                    GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManagerScript>().MuteSounds(true);
                    GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Buttons/SoundMuted");
                }
                else
                {
                    GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicScript>().MuteMusic(false);
                    GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManagerScript>().MuteSounds(false);
                    GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Buttons/Sound");
                }
                break;

            case "GoBackSelectWordButton":
                SceneManager.LoadScene("MenuSelectWordScene");
                break;

            case "TimeAttackButton":
                SceneManager.LoadScene("MenuSelectWordScene");
                selectedGameMode = "TimeAttack";
                break;

            case "LearningButton":
                SceneManager.LoadScene("MenuSelectWordScene");
                selectedGameMode = "Learning";
                break;

            case "SettingsButton":
                break;

            case "QuitButton":
                Application.Quit();
                Debug.Log("Quitting game");
                break;

            case "Hiragana":
            case "Katakana":
            case "Kanji":
            case "Vocabulary":
                //allow the game to choose the correct folder which has the same name as the button
                typeOfWords = EventSystem.current.currentSelectedGameObject.name;

                SceneManager.LoadScene("MenuDifficultyScene");
                break;

            case "StartButton":
            case "ReplayButton":
                ResetScore();
                if (selectedGameMode == "TimeAttack")
                {
                    SceneManager.LoadScene("CountdownScene");
                }

                else if (selectedGameMode == "Learning")
                {
                    SceneManager.LoadScene("TimeAttackScene");
                }
                break;

            default:
                break;                
        }

        if (nameOfClickedButton != "SoundButton")
            SoundManagerScript.PlaySoundDrum();
    }

    //reset score and highscore
    public static void ResetScore()
    {
        HighscoreScript.highscoreValue = 0;
        ScoreScript.counterValue = 0;
        CorrectWordsCounterScript.correctWordsValue = 0;
    }

    public static void DestroyAudioSources()
    {
        Destroy(GameObject.FindGameObjectWithTag("MusicManager"));
        Destroy(GameObject.FindGameObjectWithTag("SoundManager"));
    }
}
