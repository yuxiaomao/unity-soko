public class LevelOverlayUIHandler : UIHandler
{
    public static LevelOverlayUIHandler Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public static void OnClickPause()
    {
        GameManager.OpenPauseMenu();
    }
}
