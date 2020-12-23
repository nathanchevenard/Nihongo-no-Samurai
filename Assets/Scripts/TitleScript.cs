using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleScript : MonoBehaviour
{

    public TMP_Text TitleText;

    // Start is called before the first frame update
    void Start()
    {
        if (MenuButtonScript.selectedGameMode == "TimeAttack")
            TitleText.text = "Time Attack Mode : " + MenuButtonScript.typeOfWords;
        else
            TitleText.text = MenuButtonScript.selectedGameMode + " Mode : " + MenuButtonScript.typeOfWords;
    }

    // Update is called once per frame
    void Update()
    {
 
    }
}
