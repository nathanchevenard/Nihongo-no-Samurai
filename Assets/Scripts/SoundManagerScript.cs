using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{

    public static AudioClip correctSound, wrongSound, drumSound;
    public static Object[] wordsSound;
    static AudioSource audioSource;
    
    public static float soundVolume = 0.03f;

    public static bool isMuted = false;

    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(this.gameObject);

        correctSound = Resources.Load<AudioClip>("Sounds/slashCorrect");
        wrongSound = Resources.Load<AudioClip>("Sounds/slashWrong");
        drumSound = Resources.Load<AudioClip>("Sounds/DaikoDrum1Sec");

        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySoundClic(string wordState)
    {
        if (!isMuted)
            soundVolume = 0.03f;

        switch (wordState)
        {
            case "correct":
                audioSource.PlayOneShot(correctSound, soundVolume);
                break;

            case "wrong":
                audioSource.PlayOneShot(wrongSound, soundVolume);
                break;
        }
    }

    public static void LoadWordClips(string typeofwords)
    {
        switch (typeofwords)
        {
            case "Hiragana":
            case "Katakana":
                wordsSound = Resources.LoadAll("Sounds/Kana", typeof(AudioClip));
                break;

            case "Kanji":
                wordsSound = Resources.LoadAll("Sounds/Kanji", typeof(AudioClip));
                break;

            case "Vocabulary":
                wordsSound = Resources.LoadAll("Sounds/Vocabulary", typeof(AudioClip));
                break;
        }
    }

    public static void PlaySoundWord(string word)
    {
        if (!isMuted)
            soundVolume = 0.8f;

        //in case of the word being at the end of the line in the file
        word = word.Replace("\r", "").Replace("\n", "");

        foreach (AudioClip clip in wordsSound)
        {
            if (clip.name == word)
            {
                audioSource.PlayOneShot(clip, soundVolume);
                Debug.Log(clip.name);
            }
        }
    }

    public static void PlaySoundDrum()
    {
        if (!isMuted)
            soundVolume = 0.05f;

        audioSource.PlayOneShot(drumSound, soundVolume);
    }

    public static void PlaySoundDrumLoud()
    {
        if (!isMuted)
            soundVolume = 0.1f;

        audioSource.PlayOneShot(drumSound, soundVolume);
    }

    public void MuteSounds(bool choice)
    {
        if (choice == true && isMuted == false)
        {
            soundVolume = 0;
            isMuted = true;
        }

        else if (choice == false && isMuted == true)
        {
            isMuted = false;
        }
    }

    static Object instance = null;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
