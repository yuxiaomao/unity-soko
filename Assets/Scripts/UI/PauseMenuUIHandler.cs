using UnityEngine;

public class PauseMenuUIHandler : UIHandler
{
    public static PauseMenuUIHandler Instance { get; private set; }
    private static MenuButtonsGenerator menuButtonsGenerator;

    private void Awake()
    {
        Instance = this;
        menuButtonsGenerator = GetComponentInChildren<MenuButtonsGenerator>();
        Hide();
    }

    public override void Show()
    {
        base.Show();
        // Delayed button generation
        if (menuButtonsGenerator.GeneratedButtons == null)
        {
            menuButtonsGenerator.Generate();
            defaultSelected = menuButtonsGenerator.GeneratedButtons[0].gameObject;
        }
    }

    // UI element call by UnityEvent has 0 parameter

    public static void OnClickResume()
    {
        GameManager.ClosePauseMenu();
    }

    public static void OnClickReset()
    {
        Debug.Log("Reset");
    }

    public static void OnClickMainMenu()
    {
        GameManager.OpenMainMenu();
    }

}
