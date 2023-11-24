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

    private static List<AudioSource> audioSources;
    static float delay = 0f;

    public static Toggle humanToggle;
    public static Button humanButton;
    // Start is called before the first frame update
    void Start()
    {
        screenManager = GameObject.Find("ScreenManager").GetComponent<screenManager>();
        volumeSlider = GameObject.Find("GameManager/ScreenManager/Options/VolumeSlider").GetComponent<Slider>();
        musicSource = GameObject.Find("GameManager/MenuManager/Music").GetComponent<AudioSource>();

        humanToggle = GameObject.Find("GameManager/ScreenManager/MainMenu/ConfirmHuman/Toggle").GetComponent<Toggle>();
        humanButton = GameObject.Find("GameManager/ScreenManager/MainMenu/ConfirmHuman/PLAY").GetComponent<Button>();

        sfxVolumeSlider = GameObject.Find("GameManager/ScreenManager/Options/SfxVolumeSlider").GetComponent<Slider>();

        musicSource.Play();
        musicSource.volume = volumeSlider.value;
    }

    // Update is called once per frame
    void Update()
    {
        float sliderValue = volumeSlider.value;
        musicSource.volume = sliderValue;
    }

    public static void TogglePauseAudio()
    {
        if(gameManager.paused)
        {
            musicSource.Pause();
        }
        else
        {
            musicSource.UnPause();

            float sliderValue = volumeSlider.value;
            musicSource.volume = sliderValue;
        }

    }

    public void ToggleFullScreen()
    {
        if (Screen.fullScreen == true) Screen.fullScreen = false;
        else Screen.fullScreen = true;
    }

    public void ConfirmNotRobot()
    {
        if (humanToggle.isOn) humanButton.interactable = true;

        if (!humanToggle.isOn) humanButton.interactable = false;
    }

    public void ShowConfirmNotRobot()
    {
        screenManager.RobotScreen();
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
