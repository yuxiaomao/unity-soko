using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Generate multiple menu buttons on the attached game object
/// </summary>
public class MenuButtonsGenerator : MonoBehaviour
{

    [SerializeField] private Button ButtonPrefab;
    [SerializeField] private ButtonInfo[] ButtonInfos;
    public Button[] GeneratedButtons { get; private set; }

    [System.Serializable]
    private class ButtonInfo
    {
        public string displayText;
        public Button.ButtonClickedEvent OnClick;
    }

    public void Generate()
    {
        GeneratedButtons = new Button[ButtonInfos.Length];
        for (int i = 0; i < ButtonInfos.Length; i++)
        {
            Button newButton = Instantiate(ButtonPrefab, transform);
            newButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = ButtonInfos[i].displayText;
            newButton.onClick = ButtonInfos[i].OnClick;
            GeneratedButtons[i] = newButton;
        }
    }
}
