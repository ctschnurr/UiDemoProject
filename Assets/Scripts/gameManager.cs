using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    public enum sceneState
    {
        titleScene,
        gameplayScene
    }

    screenManager screenManager;
    // Start is called before the first frame update
    void Start()
    {
        screenManager = GameObject.Find("ScreenManager").GetComponent<screenManager>();
        screenManager.SetupScreens();
        screenManager.SetScreen(screenManager.Screen.mainMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void SetScene(sceneState input)
    {
        switch(input)
        {
            case sceneState.titleScene:
                SceneManager.LoadScene(sceneName: "TitleScene");
                break;

            case sceneState.gameplayScene:
                SceneManager.LoadScene(sceneName: "GameplayScene");
                break;
        }
    }
}
