using UnityEngine;

public class MainMenuUIHandler : MonoBehaviour
{
    // UI element call by UnityEvent has 0 parameter

    public static void OnClickStart()
    {
        GameManager.LoadSceneLevelN(0);
    }

    public static void OnClickExit()
    {
        GameManager.ExitGame();
    }
}
