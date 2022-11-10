public class PauseMenuUIHandler : UIHandler
{
    public static PauseMenuUIHandler Instance { get; private set; }
    private static MenuButtonsGenerator menuButtonsGenerator;
    private MenuButtonsGenerator.ButtonInfo[] ButtonInfos;

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
            InitMenuButtonInfos();
            menuButtonsGenerator.SetButtonInfos(ButtonInfos);
            menuButtonsGenerator.Generate();
            defaultSelected = menuButtonsGenerator.GeneratedButtons[0].gameObject;
        }
        base.Show();
    }

    private void InitMenuButtonInfos()
    {
        ButtonInfos = new MenuButtonsGenerator.ButtonInfo[3];
        ButtonInfos[0] = new MenuButtonsGenerator.ButtonInfo
        {
            localizedDisplayTextEntry = "UIString/PauseMenu/Resume",
            OnClickListener = () => OnClickResume(),
        };
        ButtonInfos[1] = new MenuButtonsGenerator.ButtonInfo
        {
            localizedDisplayTextEntry = "UIString/PauseMenu/Reset",
            OnClickListener = () => OnClickReset(),
        };
        ButtonInfos[2] = new MenuButtonsGenerator.ButtonInfo
        {
            localizedDisplayTextEntry = "UIString/PauseMenu/MainMenu",
            OnClickListener = () => OnClickMainMenu(),
        };
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
