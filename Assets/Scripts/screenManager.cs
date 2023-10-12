using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class screenManager : MonoBehaviour
{
    public enum Screen
    {
        mainMenu,
        options,
        gameplay,
        pause,
        win,
        lose,
        credits
    }

    static GameObject mainMenuObject;
    static GameObject optionsObject;
    static GameObject gameplayObject;
    static GameObject pauseObject;
    static GameObject winObject;
    static GameObject loseObject;
    static GameObject creditsObject;

    static TextMeshProUGUI gunStatus;

    public static Screen currentScreen;
    public static Screen lastScreen;

    static Image damageOverlay;

    static List<GameObject> screenList;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetupScreens()
    {
        mainMenuObject = transform.Find("MainMenu").gameObject;
        optionsObject = transform.Find("Options").gameObject;
        gameplayObject = transform.Find("Gameplay").gameObject;
        pauseObject = transform.Find("Pause").gameObject;
        winObject = transform.Find("Win").gameObject;
        loseObject = transform.Find("Lose").gameObject;
        creditsObject = transform.Find("Credits").gameObject;

        screenList = new List<GameObject>
        {
            mainMenuObject,
            optionsObject,
            gameplayObject,
            pauseObject,
            winObject,
            loseObject,
            creditsObject
        };

        gunStatus = gameplayObject.transform.Find("GunStatus").GetComponent<TextMeshProUGUI>();
        damageOverlay = gameplayObject.transform.Find("DamageOverlay").GetComponent<Image>();

        ClearScreen();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void SetScreen(Screen input)
    {
        lastScreen = currentScreen;
        ClearScreen();

        switch(input)
        {
            case Screen.mainMenu:
                mainMenuObject.SetActive(true);
                currentScreen = Screen.mainMenu;
                break;

            case Screen.options:
                optionsObject.SetActive(true);
                currentScreen = Screen.options;
                break;

            case Screen.gameplay:
                gameplayObject.SetActive(true);
                currentScreen = Screen.gameplay;
                break;

            case Screen.pause:
                pauseObject.SetActive(true);
                currentScreen = Screen.pause;
                break;

            case Screen.win:
                winObject.SetActive(true);
                currentScreen = Screen.win;
                break;

            case Screen.lose:
                loseObject.SetActive(true);
                currentScreen = Screen.lose;
                break;

            case Screen.credits:
                creditsObject.SetActive(true);
                currentScreen = Screen.credits;
                break;
        }

    }

    public static void ClearScreen()
    {
        foreach(GameObject screen in screenList) if (screen.activeSelf) screen.SetActive(false);
    }

    public static void UpdateGunStatus(string input)
    {
        gunStatus.text = "Status: " + input;
    }

    public static void ApplyDamageOverlay(float health)
    {
        Color tempcolor = damageOverlay.color;
        tempcolor.a = 100 - health;
        damageOverlay.color = tempcolor;
    }
}
