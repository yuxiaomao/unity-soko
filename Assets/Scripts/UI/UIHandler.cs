using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// UIHandler base class
/// </summary>
public class UIHandler : MonoBehaviour
{
    protected GameObject defaultSelected = default;

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Hide UI component by setting active to false,
    /// remove current selected belongs this UI
    /// </summary>
    public virtual void Hide()
    {
        gameObject.SetActive(false);
        if (EventSystem.current != null &&
            EventSystem.current.currentSelectedGameObject != null &&
            EventSystem.current.currentSelectedGameObject.transform.IsChildOf(this.transform))
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    /// <summary>
    /// If EventSystem does not have current select element,
    /// and defaultSelected object is not default,
    /// select the defaultSelected game object.
    /// </summary>
    public virtual void SelectDefaultGameObject()
    {
        if (defaultSelected != default)
        {
            // For now we're sure that at east one button will be generated
            EventSystem.current.SetSelectedGameObject(defaultSelected);
        }
    }
}
