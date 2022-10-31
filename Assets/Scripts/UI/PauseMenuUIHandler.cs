public class PauseMenuUIHandler : UIHandler
{
    public static PauseMenuUIHandler Instance { get; private set; }
    private static MenuButtonsGenerator menuButtonsGenerator;

    private void Start()
    {
        Instance = this;
        menuButtonsGenerator = GetComponentInChildren<MenuButtonsGenerator>();
        Hide();
    }

    public override void Show()
    {
        // Delayed button generation
        if (menuButtonsGenerator.GeneratedButtons == null)
        {
            menuButtonsGenerator.Generate();
            defaultSelected = menuButtonsGenerator.GeneratedButtons[0].gameObject;
        }
        base.Show();
    }

    // UI element call by UnityEvent has 0 parameter

    public static void OnClickResume()
    {
        GameManager.ClosePauseMenu();
    }

    public static void OnClickReset()
    {
        GameManager.ResetCurrentLevel();
        GameManager.ClosePauseMenu();
    }

    public static void OnClickMainMenu()
    {
        GameManager.OpenMainMenu();
    }

}
