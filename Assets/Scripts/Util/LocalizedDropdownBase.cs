using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Extended TMP_Dropdown, rewrite CreateDropdownList function
/// </summary>
public class LocalizedDropdownBase : TMP_Dropdown
{
    private LocalizedTextBase localizedCaptionText;

    protected override void Awake()
    {
        base.Awake();
        localizedCaptionText = captionText.GetComponent<LocalizedTextBase>();
        onValueChanged.AddListener(UpdateSelectedLabel);
    }

    /// <summary>
    /// Called after CreateDropdownList and CreateItem if open dropdown with mouose
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        LocalizeDropdownList();
    }

    /// <summary>
    /// Called after CreateDropdownList and CreateItem if open dropdown with keyboard
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnSubmit(BaseEventData eventData)
    {
        base.OnSubmit(eventData);
        LocalizeDropdownList();
    }

    /// <summary>
    /// Localize generated dropdown list
    /// </summary>
    private void LocalizeDropdownList()
    {
        LocalizedTextBase[] localized = transform.Find("Dropdown List").GetComponentsInChildren<LocalizedTextBase>();
        for (int i = 0; i < localized.Length; i++)
        {
            localized[i].SetTableEntry(localized[i].gameObject.GetComponent<TextMeshProUGUI>().text);
        }
    }

    /// <summary>
    /// OnValueChanged, update captionText
    /// </summary>
    /// <param name="index"></param>
    private void UpdateSelectedLabel(int index)
    {
        Debug.Log("UpdateSelectedLabelWithIndex:" + index + "/" + options.Count);
        if (options.Count > 0 && index < options.Count)
        {
            localizedCaptionText.SetTableEntry(options[index].text);
            localizedCaptionText.Refresh();
        }
    }

    public void UpdateSelectedLabel(string entry)
    {
        localizedCaptionText.SetTableEntry(entry);
        localizedCaptionText.Refresh();
    }
}
