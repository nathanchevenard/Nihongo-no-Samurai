using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ActivateKanjisScript : MonoBehaviour
{
    public Toggle kanjisToggle;
    public Dropdown translationRomajiDropdown;
    public Dropdown vocabCategoriesDropdown;

    private List<string> vocabFileList = new List<string>();

    private string tempTypeOfWords;

    public static bool kanjisActivated = false;

    //0 = romaji, 1 = translation
    public static int guessWordWithTranslation = 0;

    public static string selectedVocabCategory;

    // Start is called before the first frame update
    void Start()
    {
        tempTypeOfWords = MenuButtonScript.typeOfWords;

        //hides elements based on the loaded scene
        if (MenuButtonScript.selectedGameMode != "TimeAttack")
        {
            HideElements("ShowOnTimeAttack");
        }

        if (MenuButtonScript.selectedGameMode != "Learning")
        {
            HideElements("ShowOnLearning");
        }

        if ((MenuButtonScript.typeOfWords == "Hiragana" || MenuButtonScript.typeOfWords == "Katakana") || MenuButtonScript.selectedGameMode == "Learning")
        {
            HideElements("ShowOnNoKanaTA");
        }

        if ((MenuButtonScript.typeOfWords == "Hiragana" || MenuButtonScript.typeOfWords == "Katakana"))
        {
            HideElements("ShowOnNoKana");
        }

        if (MenuButtonScript.typeOfWords != "Vocabulary")
        {
            HideElements("ShowOnVocab");
        }

        kanjisToggle.onValueChanged.AddListener(delegate
        {
            ChangeToggleValue();
        });

        translationRomajiDropdown.onValueChanged.AddListener(delegate
        {
            ChangeTranslationDropdownValue();
        });

        vocabCategoriesDropdown.onValueChanged.AddListener(delegate
        {
            ChangeDropdownVocabValue();
        });

        //gets the list of vocab or kanji categories (stored manually in a txt file)

        UpdateList();
    }

    void OnEnable()
    {
        vocabCategoriesDropdown.value = vocabFileList.IndexOf(selectedVocabCategory);
    }

    void UpdateList()
    {
        TextAsset vocabListTextAsset;

        if (MenuButtonScript.typeOfWords == "Vocabulary")
            vocabListTextAsset = Resources.Load("Words/Vocabulary/vocabList") as TextAsset;

        else
            vocabListTextAsset = Resources.Load("Words/Kanji/kanjiList") as TextAsset;

        //clears previous list, resets settings
        guessWordWithTranslation = translationRomajiDropdown.value;
        kanjisActivated = kanjisToggle.isOn;
        vocabFileList.Clear();
        selectedVocabCategory = null;

        var lines = vocabListTextAsset.text.Split(',');

        //store each category in a list
        foreach (var line in lines)
        {
            StringBuilder newText = new StringBuilder(line.Length * 2);
            newText.Append(line[0]);
            for (int i = 1; i < line.Length; i++)
            {
                if (char.IsUpper(line[i]) && line[i - 1] != ' ')
                    newText.Append(' ');
                newText.Append(line[i]);
            }

            //stores the first encountered category as the default one
            if (selectedVocabCategory == null)
                selectedVocabCategory = newText.ToString();

            vocabFileList.Add(newText.ToString());
        }

        //adds all categories as options in the associated dropdown
        vocabCategoriesDropdown.AddOptions(vocabFileList);
    }


    void HideElements(string tag)
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in gameObjects)
        {
            obj.SetActive(false);
        }
    }

    void ChangeToggleValue()
    {
        kanjisActivated = kanjisToggle.isOn;
    }

    void ChangeTranslationDropdownValue()
    {
        guessWordWithTranslation = translationRomajiDropdown.value;
    }

    void ChangeDropdownVocabValue()
    {
        selectedVocabCategory = vocabCategoriesDropdown.options[vocabCategoriesDropdown.value].text;
    }

    void Destroy()
    {
        vocabCategoriesDropdown.onValueChanged.RemoveAllListeners();
        translationRomajiDropdown.onValueChanged.RemoveAllListeners();
        kanjisToggle.onValueChanged.RemoveAllListeners();
    }
}
