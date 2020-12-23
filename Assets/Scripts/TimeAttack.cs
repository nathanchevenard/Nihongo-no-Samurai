using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class TimeAttack : MonoBehaviour
{

    public GameObject Word;
    public GameObject WordText;
    public GameObject Character;
    public GameObject Background;
    public Text TextWordToGuess;
    public Texture2D cursorBase;

    private GameObject samurai;

    private int initialTextWordToGuessFontSize;
    TextAsset wordsTextAsset;

    //contains every objects that show/hide with pause system
    public static GameObject[] pauseObjects;
    
    //boolean that will be set to true whenever we need a new round of words
    public static bool needRefresh = false;

    public static float timerWhenClicked;

    int numberOfRandomWords = NumberWordsScript.numberOfRandomWords;

    string correctWord;

    //retains the previous word to guess to prevent having the same word more than once in a row
    string previousWordToGuess = "";
    List<string> alreadyUsedWords = new List<string>();

    //variables used for Learning Mode
    string newRandomCorrectWord = "一";
    string newRandomCorrectWordRomaji = "ichi";
    string newRandomCorrectWordTranslated = "un";
    int nbWordsToLearn = NumberWordsScript.numberWordsToLearn;
    public static int activeNbWordsToLearn;
    bool isLearningWord = false;
    List<string> alreadyLearntWords = new List<string>();
    int nbIterationsPerWord = 0;

    bool learningSound = false;

    void Start()
    {
        initialTextWordToGuessFontSize = TextWordToGuess.fontSize;

        //hides pause elements
        Time.timeScale = 1;
        pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        hidePaused();

        DestroyWords();

        UnityEngine.Cursor.SetCursor(cursorBase, Vector2.zero, CursorMode.ForceSoftware);

        switch (MenuButtonScript.typeOfWords)
        {
            case "Vocabulary":
                string pathVocabFile = "Words/Vocabulary/";

                if (ActivateKanjisScript.kanjisActivated)
                    pathVocabFile += "WithKanjis/";

                else
                    pathVocabFile += "NoKanjis/";

                pathVocabFile += ActivateKanjisScript.selectedVocabCategory.Replace(" ", string.Empty);
                wordsTextAsset = Resources.Load(pathVocabFile) as TextAsset;
                break;

            case "Kanji":
                string pathKanjiFile = "Words/Kanji/";
                pathKanjiFile += ActivateKanjisScript.selectedVocabCategory.Replace(" ", string.Empty);
                wordsTextAsset = Resources.Load(pathKanjiFile) as TextAsset;
                break;

            case "Hiragana":
                wordsTextAsset = Resources.Load("Words/alphabetHiragana") as TextAsset;
                break;

            case "Katakana":
                wordsTextAsset = Resources.Load("Words/alphabetKatakana") as TextAsset;
                break;
        }

        SoundManagerScript.LoadWordClips(MenuButtonScript.typeOfWords);

        if (wordsTextAsset == null)
            wordsTextAsset = Resources.Load("Words/default") as TextAsset;

        //resets the number of words to learn
        activeNbWordsToLearn = nbWordsToLearn;

        TimerScript.SetEnabled();       

        //instantiates the character in the middle of the screen
        samurai = Instantiate(Character, new Vector3(0, 25, 0), Quaternion.identity);
        samurai.transform.SetParent(GameObject.Find("Canvas").transform, false);

        GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicScript>().ResetMusicVolume();

        if (MenuButtonScript.selectedGameMode == "TimeAttack")
        {
            GenerateWords();
            
            GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicScript>().PlayTimeAttackMusic();
        }

        else if (MenuButtonScript.selectedGameMode == "Learning")
        {
            samurai.SetActive(false);
            needRefresh = true;

            GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicScript>().PlayLearningMusic();
        }

        if (MenuButtonScript.typeOfWords == "Hiragana" || MenuButtonScript.typeOfWords == "Katakana")
            TextWordToGuess.fontSize = initialTextWordToGuessFontSize + 25;

        else if (ActivateKanjisScript.guessWordWithTranslation == 1)
            TextWordToGuess.fontSize = initialTextWordToGuessFontSize + 10;
    }


    void Update()
    {
        if (MenuButtonScript.selectedGameMode == "TimeAttack")
        {

            if (TimerScript.timeLeft < 0)
            {
                DestroyWords();
            }


            if (needRefresh && GameObject.FindGameObjectsWithTag("Word").Length > 1)
            {
                DestroyWords(correctWord);
            }

            if (needRefresh && TimerScript.timeLeft < timerWhenClicked - 0.70f && GameObject.FindGameObjectsWithTag("Word").Length == 1)
            {
                DestroyWords();
                CharacterAnimation.stopAttackAnimation();
            }

            //generates new words
            if (needRefresh && TimerScript.timeLeft < timerWhenClicked - 1)
            {
                DestroyWords();
                needRefresh = false;
                GenerateWords();
            }
        }

        else if (MenuButtonScript.selectedGameMode == "Learning")
        {
            LearningSequence();            
        }

        //pauses or resumes game when pressing Esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                showPaused();                
            }
            else if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                hidePaused();
            }
        }
    }

    void OnEnable()
    {

    }


    //generates a bunch of random words, then designates one to be the word to guess
    public void GenerateWords()
    {
        List<GameObject> listOfWords = new List<GameObject>();

        for (var i = 0; i < numberOfRandomWords; i++)
        {
            GameObject randomWord = PlaceWord(i);

            //gives the object one of the available words from the selected file
            var newRandomWord = PickRandomWord(alreadyUsedWords);
            randomWord.GetComponent<SelectWord>().ReceiveWord(newRandomWord.Japanese, newRandomWord.Romaji, newRandomWord.Translated);

            listOfWords.Add(randomWord);
        }

        //picks a random word among the created ones to be the word to guess
        PickWordToGuess(numberOfRandomWords, listOfWords);      
    }


    //generates random word with a predefined correct word
    public void GenerateWords(string correctWordJapanese, string correctWordRomaji, string correctWordTranslation)
    {
        alreadyUsedWords.Add(correctWordJapanese);

        int indexOfCorrectWord = UnityEngine.Random.Range(0, numberOfRandomWords);

        for (var i = 0; i < numberOfRandomWords; i++)
        {
            GameObject randomWord = PlaceWord(i);

            if (i == indexOfCorrectWord)
            {
                //gives the object the word to guess and updates the text showing the word to guess
                randomWord.GetComponent<SelectWord>().ReceiveWord(correctWordJapanese, correctWordRomaji, correctWordTranslation);
                randomWord.GetComponent<SelectWord>().SetCorrectWord();

                string textToGuess = correctWordRomaji;

                if (MenuButtonScript.typeOfWords != "Hiragana" && MenuButtonScript.typeOfWords != "Katakana")
                    textToGuess = textToGuess + "\n" + "<i>(" + correctWordTranslation + ")</i>";                

                TextWordToGuess.GetComponent<Text>().text = textToGuess;
            }

            else
            {
                //gives the object one of the available words from the selected file
                var newRandomWord = PickRandomWord(alreadyUsedWords);
                randomWord.GetComponent<SelectWord>().ReceiveWord(newRandomWord.Japanese, newRandomWord.Romaji, newRandomWord.Translated);
            }                
        }
    }

    public GameObject PlaceWord(int i)
    {
        double offset = 0;

        if (numberOfRandomWords % 2 == 1)
        {
            offset = Math.PI / (2*numberOfRandomWords);
        }

        //places the words around the character on an ellipse
        var R = 240;
        var x = 1.6 * R * Math.Cos(i * 2 * Math.PI / numberOfRandomWords + offset);
        var y = 1 * R * Math.Sin(i * 2 * Math.PI / numberOfRandomWords + offset) - 45;

        //instantiates the word at the correct position, placed on the canvas
        GameObject randomWord = Instantiate(WordText, new Vector3(Convert.ToSingle(x), Convert.ToSingle(y), 0), Quaternion.identity);
        randomWord.transform.SetParent(GameObject.Find("Canvas").transform, false);

        return randomWord;
    }

    public void DestroyWords()
    {
        //stores every object with tag "Word" in a list, then destroy them
        GameObject[] listOfWords = GameObject.FindGameObjectsWithTag("Word");
        foreach (GameObject word in listOfWords)
        {
            GameObject.Destroy(word);
        }

        //resets the list of used words
        alreadyUsedWords.Clear();
    }


    public void DestroyWords(string correctWord)
    {
        //stores every object with tag "Word" in a list, then destroy them
        GameObject[] listOfWords = GameObject.FindGameObjectsWithTag("Word");
        foreach (GameObject word in listOfWords)
        {
            if (word.GetComponent<SelectWord>().GetTranslation() != correctWord && word.GetComponent<SelectWord>().GetRomaji() != correctWord && word.GetComponent<SelectWord>().GetWord() != correctWord)
                GameObject.Destroy(word);
        }                      

        //resets the list of used words
        alreadyUsedWords.Clear();
    }



    public (string Japanese, string Romaji, string Translated) PickRandomWord(List<string> unavailableWords)
    {
        var lines = wordsTextAsset.text.Split('\n');
        string pickedWordJapanese, pickedWordRomaji, pickedWordTranslated;
        int count = 0;

        //counts number of lines
        foreach (var line in lines)
        {
            count++;
        }

        //picks a word that isn't already used
        do
        {
            int randomLine = UnityEngine.Random.Range(0, count);
            pickedWordJapanese = lines[randomLine].Split(',')[0];
            pickedWordRomaji = lines[randomLine].Split(',')[1];
            if (MenuButtonScript.typeOfWords != "Hiragana" && MenuButtonScript.typeOfWords != "Katakana")
                pickedWordTranslated = lines[randomLine].Split(',')[2];
            else
                pickedWordTranslated = "";

        } while (unavailableWords.Contains(pickedWordJapanese));

        //puts the selected word in the list of used words
        unavailableWords.Add(pickedWordJapanese);

        return (pickedWordJapanese, pickedWordRomaji, pickedWordTranslated);
    }


    //selects a random int representing the index of one of the created words, and gives him the status of "correct word"
    public void PickWordToGuess(int nbWords, List<GameObject> listOfWords)
    {
        int indexToReturn = 0;

        //while we don't have a correct word different from the last one
        do
        {
            indexToReturn = UnityEngine.Random.Range(0, nbWords);
        }
        while (listOfWords[indexToReturn].GetComponent<SelectWord>().GetWord() == previousWordToGuess);

        //goes through the list of words until setting the word to "correct"
        for (var i = 0; i < numberOfRandomWords; i++)
        {
            if (i == indexToReturn)
            {
                listOfWords[indexToReturn].GetComponent<SelectWord>().SetCorrectWord();

                //places the word to guess (translation of japanese word) under the character
                if (ActivateKanjisScript.guessWordWithTranslation == 1)
                    correctWord = TextWordToGuess.GetComponent<Text>().text = listOfWords[indexToReturn].GetComponent<SelectWord>().GetTranslation();                  
                
                //same with romaji version, and its translation if there is one
                else
                {
                    string textToGuess = listOfWords[indexToReturn].GetComponent<SelectWord>().GetRomaji();

                    if (MenuButtonScript.typeOfWords != "Hiragana" && MenuButtonScript.typeOfWords != "Katakana")
                        textToGuess = textToGuess + "\n<i>(" + listOfWords[indexToReturn].GetComponent<SelectWord>().GetTranslation() + ")</i>";

                    correctWord = listOfWords[indexToReturn].GetComponent<SelectWord>().GetRomaji();

                    TextWordToGuess.GetComponent<Text>().text = textToGuess;            
                }
                    
                break;
            }
        }

        //replaces the old previous word with the correct word we just picked
        previousWordToGuess = listOfWords[indexToReturn].GetComponent<SelectWord>().GetWord();
    }


    //shows objects with ShowOnPause tag
    public static void showPaused()
    {
        GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicScript>().ChangeMusicVolume(MusicScript.pauseMusicVolume);
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(true);
        }
    }

    //hides objects with ShowOnPause tag
    public static void hidePaused()
    {
        GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicScript>().ResetMusicVolume();
        foreach (GameObject g in pauseObjects)
        {
            g.SetActive(false);
        }
    }

    //used in learning, the word appears with its translation (romaji and meaning) and is pronounced
    public void ShowWordToGuess(string word)
    {      
        //plays the sound of the word a second time
        if (TimerScript.timeLeft < 3.5f && !learningSound)
        {
            SoundManagerScript.PlaySoundWord(word);
            learningSound = true;

        }

        //before generating random word, have a second with just the character on screen
        if (TimerScript.timeLeft < 1f && GameObject.FindGameObjectsWithTag("Word").Length > 0)
        {
            DestroyWords();

            samurai.SetActive(true);
        }
    }

    public static void ClearTextWordToGuess()
    {
        GameObject.Find("TextWordToGuess").GetComponent<Text>().text = "";
    }

    public void LearningSequence()
    {
        if (needRefresh)
        {
            //when a new word needs to be selected in order to be learnt
            if (nbIterationsPerWord == 0)
            {
                ClearTextWordToGuess();

                if (TimerScript.timeLeft < 0)
                {
                    activeNbWordsToLearn--;
                    needRefresh = false;

                    //when enough words have been learnt, go to "game over" scene
                    if (activeNbWordsToLearn < 0)
                    {
                        SceneManager.LoadScene("GameOverScene");

                        TimerScript.SetDisabled();
                        return;
                    }

                    nbIterationsPerWord = 2;

                    DestroyWords();

                    samurai.SetActive(false);

                    isLearningWord = true;
                    TimerScript.timeLeft = 6;

                    //gets the word to learn and its Romaji
                    var newRandomCorrectWordTuple = PickRandomWord(alreadyLearntWords);
                    correctWord = newRandomCorrectWord = newRandomCorrectWordTuple.Japanese;
                    newRandomCorrectWordRomaji = newRandomCorrectWordTuple.Romaji;
                    newRandomCorrectWordTranslated = newRandomCorrectWordTuple.Translated;

                    //displays the word to learn
                    GameObject wordToLearn = Instantiate(WordText, new Vector3(0, -20, 0), Quaternion.identity);
                    wordToLearn.transform.SetParent(GameObject.Find("Canvas").transform, false);

                    //give the word object the correct words, sets it inactive so the player can't click on it
                    wordToLearn.GetComponent<SelectWord>().ReceiveWord(newRandomCorrectWord, newRandomCorrectWordRomaji, newRandomCorrectWordTranslated);
                    wordToLearn.GetComponent<SelectWord>().SetInactive();

                    string textToGuess = newRandomCorrectWordRomaji;
                    
                    if (MenuButtonScript.typeOfWords != "Hiragana" && MenuButtonScript.typeOfWords != "Katakana")
                        textToGuess = textToGuess + "\n" + "<i>(" + newRandomCorrectWordTranslated + ")</i>";

                    TextWordToGuess.GetComponent<Text>().text = textToGuess;

                    SoundManagerScript.PlaySoundWord(newRandomCorrectWordRomaji);
                }

                //clears the previous words if there were any
                else if (GameObject.FindGameObjectsWithTag("Word").Length > 1 && TimerScript.timeLeft > 0f)
                {
                    DestroyWords(correctWord);
                }

                else if (TimerScript.timeLeft < 0.5f && GameObject.FindGameObjectsWithTag("Word").Length != 0)
                {
                    DestroyWords();
                    CharacterAnimation.stopAttackAnimation();
                }
            }

            
            //when you need another roll of words for the ongoing learnt word
            else if (nbIterationsPerWord > 0)
            {
                //little pause between each word with no random words on the screen

                //clears the previous words if there were any
                if (GameObject.FindGameObjectsWithTag("Word").Length > 1 && TimerScript.timeLeft > 0f)
                {
                    DestroyWords(correctWord);
                }

                else if (TimerScript.timeLeft < 0.5f && GameObject.FindGameObjectsWithTag("Word").Length != 0)
                {
                    DestroyWords();
                    CharacterAnimation.stopAttackAnimation();
                }

                //generates new random words while keeping the word to learn as the correct one
                else if (TimerScript.timeLeft < 0f)
                {
                    nbIterationsPerWord--;
                    needRefresh = false;
                    GenerateWords(newRandomCorrectWord, newRandomCorrectWordRomaji, newRandomCorrectWordTranslated);
                }
            }
        }
              
        //one loop of the process for learning one word
        if (activeNbWordsToLearn >= 0 && isLearningWord)
        {
            //sets the word and the character
            ShowWordToGuess(newRandomCorrectWordRomaji);            

            //repeats the word twice while showing the word during a given time
            if (TimerScript.timeLeft < 0)
            {                
                TimerScript.timeLeft = 1.5f;

                GenerateWords(newRandomCorrectWord, newRandomCorrectWordRomaji, newRandomCorrectWordTranslated);

                isLearningWord = false;
                learningSound = false;

                samurai.SetActive(true);
            }
        }
    }
}
