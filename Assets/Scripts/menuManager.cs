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
    public static AudioSource musicSource;

    static float delay = 0f;
    // Start is called before the first frame update
    void Start()
    {
        screenManager = GameObject.Find("ScreenManager").GetComponent<screenManager>();
        volumeSlider = GameObject.Find("GameManager/ScreenManager/Options/VolumeSlider").GetComponent<Slider>();
        musicSource = GameObject.Find("GameManager/MenuManager/Music").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float sliderValue = volumeSlider.value;
        musicSource.volume = sliderValue;

        delay -= Time.deltaTime;
        if (delay <= 0 && !musicSource.isPlaying)
        {
            musicSource.Play();
            delay = -1;
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
