using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting.Antlr3.Runtime;

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
        credits,
        instructions
    }

    static GameObject mainMenuObject;
    static GameObject optionsObject;
    static GameObject gameplayObject;
    static GameObject pauseObject;
    static GameObject winObject;
    static GameObject loseObject;
    static GameObject creditsObject;
    static GameObject confirmHumanObject;
    static GameObject instructionsObject;
    static bool confirmHumanScreenOn = false;

    static GameObject fadeScreenObj;
    static GameObject fadeScreenImg;

    static GameObject instructions1;
    static GameObject instructions2;
    static GameObject instructions3;

    static Button instructionsBackButton;
    static Button instructionsPlayButton;
    static Button instructionsNextButton;

    static TextMeshProUGUI gunStatus;
    static TextMeshProUGUI playerHealth;
    static Slider playerHealthbar;
    static Slider playerStaminabar;
    static Image reloadProgress;
    static GameObject reloadProgressObj;

    public static Screen currentScreen;
    public static Screen lastScreen;

    static Image damageOverlay;

    static List<GameObject> screenList;

    static int gameplayFadeStage = 0;
    static int titleFadeStage = 0;
    static int instructionsStage = 0;

    static GameObject instructionsUp;
    static GameObject instructionsNext;
    static GameObject instructionsBack;

    static GameObject pauseBackground;

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
        confirmHumanObject = mainMenuObject.transform.Find("ConfirmHuman").gameObject;
        fadeScreenObj = transform.Find("FadeScreen").gameObject;
        fadeScreenImg = transform.Find("FadeScreen/Image").gameObject;

        instructionsObject = transform.Find("Instructions").gameObject;

        instructions1 = transform.Find("Instructions/PanelA").gameObject;
        instructions2 = transform.Find("Instructions/PanelB").gameObject;
        instructions3 = transform.Find("Instructions/PanelC").gameObject;

        instructionsBackButton = instructionsObject.transform.Find("Back").GetComponent<Button>();
        instructionsPlayButton = instructionsObject.transform.Find("Play").GetComponent<Button>();
        instructionsNextButton = instructionsObject.transform.Find("Next").GetComponent<Button>();

        pauseBackground = transform.Find("Pause/background").gameObject;
        
        fadeScreenObj.SetActive(false);

        // confirmHumanObject.SetActive(false);

        screenList = new List<GameObject>
        {
            mainMenuObject,
            optionsObject,
            gameplayObject,
            pauseObject,
            winObject,
            loseObject,
            creditsObject,
            instructionsObject
        };

        gunStatus = gameplayObject.transform.Find("GunStatus").GetComponent<TextMeshProUGUI>();
        playerHealthbar = gameplayObject.transform.Find("PlayerHealthSlider").GetComponent<Slider>();
        playerStaminabar = gameplayObject.transform.Find("PlayerStaminaSlider").GetComponent<Slider>();
        reloadProgress = gameplayObject.transform.Find("reloadProgress").GetComponent<Image>();
        damageOverlay = GameObject.Find("DamageOverlay").GetComponent<Image>();

        // reloadProgress.maxValue = FirstPersonController_Sam.ReloadTimerMax;

        reloadProgressObj = gameplayObject.transform.Find("reloadProgress").gameObject;
        reloadProgressObj.SetActive(false);

        ClearScreen();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerStats();

        reloadProgress.fillAmount = FirstPersonController_Sam.ReloadTimer / FirstPersonController_Sam.ReloadTimerMax;
    }

    public static void FadeToGameplay()
    {
        switch (gameplayFadeStage)
        {
            case 0:
                gameplayFadeStage++;
                fadeScreenObj.SetActive(true);
                LeanTween.alpha(fadeScreenImg.GetComponent<RectTransform>(), 1, 1).setOnComplete(FadeToGameplay);
                break;
            case 1:
                gameplayFadeStage++;
                gameManager.SetScene(gameManager.sceneState.gameplayScene);
                SetScreen(Screen.instructions);
                LeanTween.alpha(fadeScreenImg.GetComponent<RectTransform>(), 0, 1).setDelay(.5f).setOnComplete(FadeToGameplay);
                break;
            case 2:
                gameplayFadeStage = 0;
                fadeScreenObj.SetActive(false);
                break;
        }
    }

    public static void FadeToTitle()
    {
        switch (titleFadeStage)
        {
            case 0:
                titleFadeStage++;
                fadeScreenObj.SetActive(true);
                LeanTween.alpha(fadeScreenImg.GetComponent<RectTransform>(), 1, 1).setOnComplete(FadeToTitle).setIgnoreTimeScale(true);
                break;
            case 1:
                titleFadeStage++;
                gameManager.SetScene(gameManager.sceneState.titleScene);
                SetScreen(Screen.mainMenu);
                LeanTween.alpha(fadeScreenImg.GetComponent<RectTransform>(), 0, 1).setDelay(.5f).setOnComplete(FadeToTitle).setIgnoreTimeScale(true);
                break;
            case 2:
                titleFadeStage = 0;
                fadeScreenObj.SetActive(false);
                break;
        }
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
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                gameplayObject.SetActive(true);
                currentScreen = Screen.gameplay;
                break;

            case Screen.pause:
                LeanTween.rotateAround(pauseBackground, Vector3.forward, 360, 7f).setLoopClamp().setIgnoreTimeScale(true);
                pauseObject.SetActive(true);
                currentScreen = Screen.pause;
                break;

            case Screen.win:
                winObject.SetActive(true);
                currentScreen = Screen.win;
                break;

            case Screen.lose:
                gameplayObject.SetActive(true);
                loseObject.SetActive(true);
                currentScreen = Screen.lose;
                break;

            case Screen.credits:
                creditsObject.SetActive(true);
                currentScreen = Screen.credits;
                break;

            case Screen.instructions:
                instructionsObject.SetActive(true);
                currentScreen = Screen.instructions;

                instructions1.transform.localPosition = new Vector3(0, 0, 0);
                instructions2.transform.localPosition = new Vector3(1600, 0, 0);
                instructions3.transform.localPosition = new Vector3(1600, 0, 0);
                instructionsUp = instructions1;
                instructionsNext = instructions2;

                instructionsStage = 0;
                instructionsBackButton.interactable = false;
                instructionsNextButton.interactable = true;
                break;
        }

    }

    public static void InstructionsNext()
    {
        switch (instructionsStage)
        {
            case 0:
                instructionsStage++;
                LeanTween.moveLocal(instructionsUp, new Vector3(-1600f, 0f, 0f), 1f).setEase(LeanTweenType.easeInSine);
                LeanTween.moveLocal(instructionsNext, new Vector3(0f, 0f, 0f), 1.5f).setEase(LeanTweenType.easeOutSine);
                instructionsBack = instructions1;
                instructionsUp = instructions2;
                instructionsNext = instructions3;

                instructionsBackButton.interactable = true;
                break;

            case 1:
                instructionsStage++;
                LeanTween.moveLocal(instructionsUp, new Vector3(-1600f, 0f, 0f), 1f).setEase(LeanTweenType.easeInSine);
                LeanTween.moveLocal(instructionsNext, new Vector3(0f, 0f, 0f), 1.5f).setEase(LeanTweenType.easeOutSine);
                instructionsBack = instructions2;
                instructionsUp = instructions3;
                instructionsNext = null;

                instructionsPlayButton.interactable = true;
                instructionsNextButton.interactable = false;
                break;
        }
    }

    public static void InstructionsBack()
    {
        switch (instructionsStage)
        {
            case 1:
                instructionsStage--;
                LeanTween.moveLocal(instructionsUp, new Vector3(1600f, 0f, 0f), 1f).setEase(LeanTweenType.easeInSine);
                LeanTween.moveLocal(instructionsBack, new Vector3(0f, 0f, 0f), 1.5f).setEase(LeanTweenType.easeOutSine);
                instructionsBack = null;
                instructionsUp = instructions1;
                instructionsNext = instructions2;

                instructionsBackButton.interactable = false;
                break;

            case 2:
                instructionsStage--;
                LeanTween.moveLocal(instructionsUp, new Vector3(1600f, 0f, 0f), 1f).setEase(LeanTweenType.easeInSine);
                LeanTween.moveLocal(instructionsBack, new Vector3(0f, 0f, 0f), 1.5f).setEase(LeanTweenType.easeOutSine);
                instructionsBack = instructions1;
                instructionsUp = instructions2;
                instructionsNext = instructions3;

                instructionsNextButton.interactable = true;
                break;
        }
    }

    public static void DismissInstructions()
    {
        LeanTween.scale(instructionsObject, new Vector3(0f, 0f, 0f), 1f).setEase(LeanTweenType.easeInOutCubic).setOnComplete(DeactivateInstructions);
    }

    public static void DeactivateInstructions()
    {
        instructionsObject.SetActive(false);
    }

    public static void RobotScreen()
    {
        if (confirmHumanScreenOn)
        {
            LeanTween.rotateAround(confirmHumanObject, Vector3.forward, 720, 1.5f).setEase(LeanTweenType.easeInCirc);
            LeanTween.scale(confirmHumanObject, new Vector3(0f, 0f, 0f), 1.5f).setEase(LeanTweenType.easeInOutCubic).setOnComplete(FadeToGameplay);
            confirmHumanScreenOn = false;
        }
        else
        {
            LeanTween.scale(confirmHumanObject, new Vector3(1f, 1f, 1f), 1f).setEase(LeanTweenType.easeOutElastic);
            confirmHumanScreenOn = true;
        }
    }

    public static void FadeScreenOff()
    {
        fadeScreenObj.SetActive(false);
    }

    public static void ClearScreen()
    {
        foreach(GameObject screen in screenList) if (screen.activeSelf) screen.SetActive(false);
    }

    public static void UpdateGunStatus(string input)
    {
        if(FirstPersonController_Sam.Reloading)
        {
            if(!reloadProgressObj.activeSelf) reloadProgressObj.SetActive(true);
        }
        else if (reloadProgressObj.activeSelf) reloadProgressObj.SetActive(false);

        gunStatus.text = "Status: " + input;
    }

    public static void UpdatePlayerStats()
    {
        float hp = FirstPersonController_Sam.GetPlayerHealth();
        playerHealthbar.maxValue = FirstPersonController_Sam.PlayerMaxHealth;
        playerHealthbar.value = hp;

        float stamina = FirstPersonController_Sam.PlayerStamina;
        playerStaminabar.maxValue = FirstPersonController_Sam.PlayerMaxStamina;
        playerStaminabar.value = stamina;

        if (hp <= 10)
        {
            Color tempcolor = damageOverlay.color;
            tempcolor.a = 1f - (hp / 10f);
            damageOverlay.color = tempcolor;
        }
        if(hp > 10)
        {
            Color tempcolor = damageOverlay.color;
            tempcolor.a = 0;
            damageOverlay.color = tempcolor;
        }
    }
}
