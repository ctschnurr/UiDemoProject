using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static gameManager;

public class menuManager : MonoBehaviour
{
    screenManager screenManager;
    // Start is called before the first frame update
    void Start()
    {
        screenManager = GameObject.Find("ScreenManager").GetComponent<screenManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        screenManager.SetScreen(screenManager.Screen.pause);
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

    public void BackButtion()
    {
        screenManager.SetScreen(screenManager.lastScreen);
    }

    public void SetTitleScene()
    {
        SetScene(sceneState.titleScene);
    }

    public void SetGameplayScene()
    {
        SetScene(sceneState.gameplayScene);
    }
}
