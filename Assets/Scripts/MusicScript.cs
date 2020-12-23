using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicScript : MonoBehaviour
{
    public static float initialMusicVolume;
    public static float pauseMusicVolume;
    public AudioSource music;
    AudioClip timeAttackClip, menuClip, learningClip;

    public static bool isMuted = false;

    void Start()
    {
        initialMusicVolume = music.volume;
        pauseMusicVolume = initialMusicVolume / 3;

        timeAttackClip = Resources.Load<AudioClip>("Sounds/Music/TimeAttackTheme");
        menuClip = Resources.Load<AudioClip>("Sounds/Music/MenuTheme");
        learningClip = Resources.Load<AudioClip>("Sounds/Music/LearningTheme");
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

    void Update()
    {
        Scene scene = SceneManager.GetActiveScene();

        //fades away the music during countdown
        if (scene.name == "CountdownScene" && music.volume > 0)
        {
            DecreaseVolume();
        }
    }

    public void ChangeMusicVolume(float newVolume)
    {
        if (!isMuted)
            music.volume = newVolume;
    }

    public void ResetMusicVolume()
    {
        if (!isMuted)
            music.volume = initialMusicVolume;
    }

    public void PlayTimeAttackMusic()
    {
        ResetMusicVolume();
        music.Stop();
        music.clip = timeAttackClip;
        music.Play();
    }

    public void PlayMenuMusic()
    {
        ResetMusicVolume();
        music.Stop();
        music.clip = menuClip;
        music.Play();
    }

    public void PlayLearningMusic()
    {
        music.Stop();
        music.clip = learningClip;
        music.Play();
    }

    public void DecreaseVolume()
    {
        if (!isMuted)
            music.volume -= 0.01f;
    }

    public void MuteMusic(bool choice)
    {
        if (choice == true && isMuted == false)
        {            
            music.volume = 0;
            //music.Stop();
            isMuted = true;            
        }

        else if (choice == false && isMuted == true)
        {
            if (Time.timeScale == 1)
                music.volume = initialMusicVolume;
            else
                music.volume = pauseMusicVolume;
            //music.Play();
            isMuted = false;
        }
    }
}
