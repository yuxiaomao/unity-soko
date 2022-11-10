using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;

/// <summary>
/// Base class for any string that needs to be localized,
/// associate to TextTMP (TextMeshProUGUI).
/// This class will not change text if no tableEntry specified.
/// Font is initialized by this class.
/// </summary>
public class LocalizedTextBase : MonoBehaviour
{
    [SerializeField] private string tableCollectionName = Constants.TableCollectionUIString;
    [SerializeField] private string tableEntryName;

    private LocalizeStringEvent m_StringEvent;
    private LocalizeStringEvent LocalizeStringEvent
    {
        get
        {
            if (m_StringEvent == null)
            {
                m_StringEvent = gameObject.AddComponent<LocalizeStringEvent>();
            }
            return m_StringEvent;
        }
    }
    private TextMeshProUGUI text;

    private void Start()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
        SetTableCollection(tableCollectionName);
        AddListener((str) => text.text = str);
        // If tableEntryName already exists (configured in Editor), initialize localizedStringEvent
        if ((tableEntryName != null) && (tableEntryName.Length > 0))
        {
            SetTableEntry(tableEntryName);
        }
        // If Font available
        if (LocalizationManager.CurrentFont != null)
        {
            text.font = LocalizationManager.CurrentFont;
        }
        Refresh();
    }

    public void Refresh()
    {
        LocalizeStringEvent.RefreshString();
    }

    public void SetTableCollection(string collectionName)
    {
        tableCollectionName = collectionName;
        LocalizeStringEvent.SetTable(collectionName);
    }

    public void SetTableEntry(string entryName)
    {
        tableEntryName = entryName;
        LocalizeStringEvent.SetEntry(entryName);
    }

    public void AddListener(UnityAction<string> onStringChanged)
    {
        LocalizeStringEvent.OnUpdateString.AddListener(onStringChanged);
    }

    public void SetArguments(object arguments)
    {
        if (arguments != null)
        {
            LocalizeStringEvent.StringReference.Arguments = new[] { arguments };
        }
    }
}
