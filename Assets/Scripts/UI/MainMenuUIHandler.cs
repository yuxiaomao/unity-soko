public class MainMenuUIHandler : UIHandler
{
    public static MainMenuUIHandler Instance { get; private set; }
    private static MenuButtonsGenerator menuButtonsGenerator;
    private MenuButtonsGenerator.ButtonInfo[] ButtonInfos;

    private void Start()
    {
        Instance = this;
        InitMenuButtonInfos();
        menuButtonsGenerator = GetComponentInChildren<MenuButtonsGenerator>();
        menuButtonsGenerator.SetButtonInfos(ButtonInfos);
        menuButtonsGenerator.Generate();
        defaultSelected = menuButtonsGenerator.GeneratedButtons[0].gameObject;
    }

    private void InitMenuButtonInfos()
    {
        ButtonInfos = new MenuButtonsGenerator.ButtonInfo[3];
        ButtonInfos[0] = new MenuButtonsGenerator.ButtonInfo
        {
            localizedDisplayTextEntry = "UIString/MainMenu/NewGame",
            OnClickListener = () => OnClickStart(),
        };
        ButtonInfos[1] = new MenuButtonsGenerator.ButtonInfo
        {
            localizedDisplayTextEntry = "UIString/MainMenu/LevelSelect",
            OnClickListener = () => OnClickLevelSelect(),
        };
        ButtonInfos[2] = new MenuButtonsGenerator.ButtonInfo
        {
            localizedDisplayTextEntry = "UIString/MainMenu/ExitGame",
            OnClickListener = () => OnClickExit(),
        };
    }

    // UI element call by UnityEvent has 0 parameter

    public static void OnClickStart()
    {
        GameManager.OpenLevel(0);
    }

    public static void OnClickLevelSelect()
    {
        GameManager.OpenLevelSelectMenu();
    }

    public static void OnClickExit()
    {
        GameManager.ExitGame();
    }
}
