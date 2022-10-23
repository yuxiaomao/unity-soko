using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuUIHandler : UIHandler
{
    public static MainMenuUIHandler Instance { get; private set; }
    private static MenuButtonsGenerator menuButtonsGenerator;

    private void Start()
    {
        Instance = this;
        menuButtonsGenerator = GetComponentInChildren<MenuButtonsGenerator>();
        menuButtonsGenerator.Generate();
        defaultSelected = menuButtonsGenerator.GeneratedButtons[0].gameObject;
    }

    // UI element call by UnityEvent has 0 parameter

    public static void OnClickStart()
    {
        GameManager.OpenLevel(0);
    }

    public static void OnClickLevelSelect()
    {
        // TODO
        Debug.Log("TODO: Open Level Select UI");
    }

    public static void OnClickExit()
    {
        GameManager.ExitGame();
    }
}
