using UnityEngine.UI;

public class WinOverlayUIHandler : UIHandler
{
    public static WinOverlayUIHandler Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        defaultSelected = GetComponentInChildren<Button>().gameObject;
        Hide();
    }

    public static void OnClickNext()
    {
        GameManager.OpenNextLevel();
    }
}
