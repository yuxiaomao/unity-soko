using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Generate multiple menu buttons on the attached game object
/// </summary>
public class MenuButtonsGenerator : MonoBehaviour
{

    [SerializeField] private Button ButtonPrefab;
    [SerializeField] private ButtonInfo[] ButtonInfos;

    [System.Serializable]
    private class ButtonInfo
    {
        public string displayText;
        public Button.ButtonClickedEvent OnClick;
    }

    private void Start()
    {
        foreach (ButtonInfo buttonInfo in ButtonInfos)
        {
            Button newButton = Instantiate(ButtonPrefab, transform);
            newButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = buttonInfo.displayText;
            newButton.onClick = buttonInfo.OnClick;
        }
    }
}
