using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseButtonScript : MonoBehaviour
{
    public void OnClick()
    {
        string nameOfClickedButton = EventSystem.current.currentSelectedGameObject.name;

        switch (nameOfClickedButton)
        {
            case "PauseButton":
                Time.timeScale = 0;
                TimeAttack.showPaused();
                break;

            case "ResumeButtonPause":
                Time.timeScale = 1;
                TimeAttack.hidePaused();
                break;

            case "RestartButtonPause":
                TimeAttack.needRefresh = false;
                if (MenuButtonScript.selectedGameMode == "TimeAttack")
                {
                    SceneManager.LoadScene("CountdownScene");
                }
                else if (MenuButtonScript.selectedGameMode == "Learning")
                {
                    SceneManager.LoadScene("TimeAttackScene");
                }
                MenuButtonScript.ResetScore();
                break;

            case "QuitgameButtonPause":
                TimeAttack.needRefresh = false;
                SceneManager.LoadScene("MenuScene");
                GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicScript>().PlayMenuMusic();
                break;

            default:
                break;
        }
    }
}