using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuUIHandler : UIHandler
{
    public static PauseMenuUIHandler Instance { get; private set; }
    private static MenuButtonsGenerator menuButtonsGenerator;

    private void Awake()
    {
        Instance = this;
        menuButtonsGenerator = GetComponentInChildren<MenuButtonsGenerator>();
        gameObject.SetActive(false);
    }

    public override void Show()
    {
        base.Show();
        // Delayed button generation
        if (menuButtonsGenerator.GeneratedButtons == null)
        {
            menuButtonsGenerator.Generate();
        }
    }

    /// <summary>
    /// If EventSystem does not have current select element, select one by default
    /// </summary>
    public static void SelectDefaultGameObject()
    {
        if (EventSystem.current.currentSelectedGameObject == null ||
            !EventSystem.current.currentSelectedGameObject.activeInHierarchy)
        {
            // For now we're sure that at east one button will be generated
            EventSystem.current.SetSelectedGameObject(menuButtonsGenerator.GeneratedButtons[0].gameObject);
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
