using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manage user input with Player Input component.
/// Should probably not be called by other script, except GameManager for action map control on scene/UI change.
/// </summary>
public partial class UserInputManager : MonoBehaviour
{
    private PlayerInput playerInput;

    private void Start()
    {
        GameManager.AssertIsChild(gameObject);
        playerInput = gameObject.GetComponent<PlayerInput>();
    }

    /// <summary>
    /// User input action map control by GameManager
    /// </summary>
    public void DeactivateUserInput()
    {
        playerInput.DeactivateInput();
    }

    /// <summary>
    /// User input action map control by GameManager
    /// </summary>
    public void ActivateUserInput()
    {
        playerInput.ActivateInput();
    }

    /// <summary>
    /// User input action map control by GameManager
    /// </summary>
    public void ActivateUserInput(string actionMapName)
    {
        playerInput.SwitchCurrentActionMap(playerInput.actions.FindActionMap(actionMapName).name);
        playerInput.ActivateInput();
    }
}
