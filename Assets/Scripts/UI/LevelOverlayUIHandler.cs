public class LevelOverlayUIHandler : UIHandler
{
    public static LevelOverlayUIHandler Instance { get; private set; }

    private void Start()
    {
        Instance = this;
    }

    public static void OnClickPause()
    {
        GameManager.OpenPauseMenu();
    }
}
