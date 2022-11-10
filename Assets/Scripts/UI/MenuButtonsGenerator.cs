using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Generate multiple menu buttons on the attached game object
/// </summary>
public class MenuButtonsGenerator : MonoBehaviour
{
    /// <summary>
    /// Specify generated button info,
    /// with either displayText (not localized) or localizedDisplayTextEntry (localized)
    /// </summary>
    [System.Serializable]
    public class ButtonInfo
    {
        public string displayText;
        public string localizedDisplayTextEntry;
        public object localizedDisplayTextArguments;
        public UnityAction OnClickListener;
    }

    [SerializeField] private Button ButtonPrefab;
    [SerializeField] private ButtonInfo[] ButtonInfos;
    public Button[] GeneratedButtons { get; private set; }

    public void SetButtonInfos(ButtonInfo[] newButtonInfos)
    {
        ButtonInfos = newButtonInfos;
    }

    public void Generate()
    {
        GeneratedButtons = new Button[ButtonInfos.Length];
        for (int i = 0; i < ButtonInfos.Length; i++)
        {
            ButtonInfo info = ButtonInfos[i];
            Button newButton = Instantiate(ButtonPrefab, transform);
            TMPro.TextMeshProUGUI textTmp = newButton.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (info.displayText != default)
            {
                textTmp.text = info.displayText;
            }
            else
            {
                LocalizedTextBase localized = textTmp.GetComponent<LocalizedTextBase>();
                localized.SetTableEntry(info.localizedDisplayTextEntry);
                localized.SetArguments(info.localizedDisplayTextArguments);
                localized.Refresh();
            }
            newButton.onClick.AddListener(info.OnClickListener);
            GeneratedButtons[i] = newButton;
        }
    }
}
