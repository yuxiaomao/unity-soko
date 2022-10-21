using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// UIHandler base class
/// </summary>
public class UIHandler : MonoBehaviour
{
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
        // When hiding, remove current selected on this UI
        if (EventSystem.current != null &&
            EventSystem.current.currentSelectedGameObject != null &&
            EventSystem.current.currentSelectedGameObject.transform.IsChildOf(this.transform))
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
