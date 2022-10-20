using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuUIHandler : MonoBehaviour
{
    private static MenuButtonsGenerator menuButtonsGenerator;

    private void Start()
    {
        menuButtonsGenerator = transform.GetComponentInChildren<MenuButtonsGenerator>();
        menuButtonsGenerator.Generate();
    }

    /// <summary>
    /// If EventSystem does not have current select element, select one by default
    /// </summary>
    public static void SelectDefaultGameObject()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            // For now we're sure that at east one button will be generated
            EventSystem.current.SetSelectedGameObject(menuButtonsGenerator.GeneratedButtons[0].gameObject);
        }
    }

    // UI element call by UnityEvent has 0 parameter

    public static void OnClickStart()
    {
        GameManager.LoadSceneLevelN(0);
    }

    public static void OnClickLevelSelect()
    {
        // TODO
        Debug.Log("TODO: Open Level Select UI");
    }

    public static void OnClickExit()
    {
        GameManager.ExitGame();
    }
}
