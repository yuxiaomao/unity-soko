using UnityEngine.UI;

public class LevelSelectMenuUIHandler : UIHandler
{
    private class LevelSelectLocalizationArgument
    {
        public string levelName;
    }

    public static LevelSelectMenuUIHandler Instance { get; private set; }
    private static MenuButtonsGenerator menuButtonsGenerator;
    /// <summary>
    /// ButtonInfos generated based on LevelManager.Level (exclus Level.None)
    /// </summary>
    private readonly MenuButtonsGenerator.ButtonInfo[] ButtonInfos = new MenuButtonsGenerator.ButtonInfo[LevelManager.LevelCount - 1];

    private void Start()
    {
        Instance = this;
        menuButtonsGenerator = GetComponentInChildren<MenuButtonsGenerator>();
        LevelManager.Level level = LevelManager.Level.Level0;
        for (int i = 0; i < ButtonInfos.Length; i++)
        {
            string levelName = Util.GetEnumStringValue(level);
            // Get level name, if level name begin with "Level"
            if (levelName.StartsWith("Level"))
            {
                levelName = levelName[5..];
            }
            LevelManager.Level levelForLambda = level;
            ButtonInfos[i] = new MenuButtonsGenerator.ButtonInfo
            {
                localizedDisplayTextEntry = "UIString/LevelSelectMenu/LevelN",
                localizedDisplayTextArguments = new LevelSelectLocalizationArgument()
                {
                    levelName = levelName,
                },
                OnClickListener = () => GameManager.OpenLevel(levelForLambda),
            };
            menuButtonsGenerator.SetButtonInfos(ButtonInfos);
            level = LevelManager.GetNextLevelOf(level);
        }
        Hide();
    }

    public override void Show()
    {
        // Delayed button generation
        if (menuButtonsGenerator.GeneratedButtons == null)
        {
            menuButtonsGenerator.Generate();
            defaultSelected = menuButtonsGenerator.GeneratedButtons[0].gameObject;
            // Mark all buttons as disable except the first one
            for (int i = 1; i < ButtonInfos.Length; i++)
            {
                menuButtonsGenerator.GeneratedButtons[i].GetComponent<Button>().interactable = false;
            }
        }
        // Button active state based on level win (skip 1st element Level.None; unlock button for next level)
        for (int i = 1; i < ButtonInfos.Length; i++)
        {
            if (GameManager.levelWin[i])
            {
                menuButtonsGenerator.GeneratedButtons[i].GetComponent<Button>().interactable = true;
            }
        }
        base.Show();
    }

    public static void OnClickExit()
    {
        GameManager.CloseLevelSelectMenu();
    }
}
