using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    public enum sceneState
    {
        titleScene,
        gameplayScene
    }

    public static bool paused = false;

    screenManager screenManager;

    // Start is called before the first frame update
    void Start()
    {
        menuManager.MusicSource.Play();

        screenManager = GameObject.Find("ScreenManager").GetComponent<screenManager>();
        screenManager.SetupScreens();

        if (SceneManager.GetActiveScene().name == "TitleScene")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            screenManager.SetScreen(screenManager.Screen.mainMenu);
        }

        if (SceneManager.GetActiveScene().name == "GameplayScene")
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            screenManager.SetScreen(screenManager.Screen.gameplay);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void SetScene(sceneState input)
    {
        screenManager.ClearScreen();

        switch(input)
        {
            case sceneState.titleScene:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                SceneManager.LoadScene("TitleScene");
                break;

            case sceneState.gameplayScene:
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                paused = false;
                SceneManager.LoadScene("GameplayScene");
                break;
        }
    }

    public static void Pause()
    {
        if(paused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            screenManager.SetScreen(screenManager.Screen.gameplay);
            paused = false;
            menuManager.TogglePauseAudio();
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
            paused = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            screenManager.SetScreen(screenManager.Screen.pause);

            menuManager.TogglePauseAudio();
        }
    }

    public static void WinGame()
    {
        paused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        screenManager.SetScreen(screenManager.Screen.win);
    }

    public static void LoseGame()
    {
        paused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        screenManager.SetScreen(screenManager.Screen.lose);
    }
}
