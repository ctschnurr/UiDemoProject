using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static gameManager;

public class menuManager : MonoBehaviour
{
    static screenManager screenManager;
    public static Slider volumeSlider;
    private static AudioSource musicSource;
    public static AudioSource MusicSource { get { return musicSource; } set { musicSource = value; } }
    public static Slider sfxVolumeSlider;
    private static AudioSource sfxSource;
    private static AudioSource sizzleSource;
    public static AudioSource SfxSource { get { return sfxSource; } set { sfxSource = value; } }
    private static List<AudioSource> audioSources;
    static float delay = 0f;
    // Start is called before the first frame update
    void Start()
    {
        screenManager = GameObject.Find("ScreenManager").GetComponent<screenManager>();
        volumeSlider = GameObject.Find("GameManager/ScreenManager/Options/VolumeSlider").GetComponent<Slider>();
        musicSource = GameObject.Find("GameManager/MenuManager/Music").GetComponent<AudioSource>();

        sfxVolumeSlider = GameObject.Find("GameManager/ScreenManager/Options/SfxVolumeSlider").GetComponent<Slider>();
        sfxSource = GameObject.Find("Player/SFXSource").GetComponent<AudioSource>();

        sizzleSource = GameObject.Find("DeathSphere/sfx").GetComponent<AudioSource>();

        audioSources = new List<AudioSource>() { musicSource, sfxSource, sizzleSource };
    }

    // Update is called once per frame
    void Update()
    {
        float sliderValue = volumeSlider.value;
        musicSource.volume = sliderValue;

        float sfxSliderValue = sfxVolumeSlider.value;
        sfxSource.volume = sfxSliderValue;
        sizzleSource.volume = sfxSliderValue;

    }

    public static void TogglePauseAudio()
    {
        if(gameManager.paused)
        {
            foreach(AudioSource audio in audioSources)
            {
                audio.Pause();
            }
        }
        else
        {
            foreach (AudioSource audio in audioSources)
            {
                audio.UnPause();
            }
        }

    }

    public void ToggleFullScreen()
    {
        if (Screen.fullScreen == true) Screen.fullScreen = false;
        else Screen.fullScreen = true;
    }

    public void SetMainMenu()
    {
        screenManager.SetScreen(screenManager.Screen.mainMenu);
    }

    public void SetOptions()
    {
        screenManager.SetScreen(screenManager.Screen.options);
    }

    public void SetPause()
    {
        gameManager.Pause();
    }

    public void SetGameplay()
    {
        screenManager.SetScreen(screenManager.Screen.gameplay);
    }

    public void SetWin()
    {
        screenManager.SetScreen(screenManager.Screen.win);
    }

    public void SetLose()
    {
        screenManager.SetScreen(screenManager.Screen.lose);
    }

    public void SetCredits()
    {
        screenManager.SetScreen(screenManager.Screen.credits);
    }

    public void SetTitleScene()
    {
        SetScene(sceneState.titleScene);
        screenManager.SetScreen(screenManager.Screen.mainMenu);
    }

    public void SetGameplayScene()
    {
        SetScene(sceneState.gameplayScene);
        screenManager.SetScreen(screenManager.Screen.gameplay);
    }

    public void NewGame()
    {
        SetScene(sceneState.gameplayScene);
        screenManager.SetScreen(screenManager.Screen.gameplay);
    }

    public void BackButton()
    {
        screenManager.SetScreen(screenManager.lastScreen);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
