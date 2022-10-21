using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// Manage user input with Player Input component.
/// Should probably not be called by other script, except GameManager for action map control on scene/UI change.
/// </summary>
public class UserInputManager : MonoBehaviour
{
    private const float MouseMoveThresh = 0.2f;
    private PlayerInput playerInput;
    // In level only
    private PlayerController m_Player;
    private PlayerController Player
    {
        get
        {
            if (m_Player == null)
            {
                m_Player = GameObject.FindGameObjectWithTag(Constants.TagPlayer).GetComponent<PlayerController>();
            }
            return m_Player;
        }
    }

    private void Awake()
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

    // Action map call by Player Input component has 1 parameter (InputAction.CallbackContext context)

    public void OnMainMenuExit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            MainMenuUIHandler.OnClickExit();
        }
    }

    public void OnMainMenuNavigationMouse(InputAction.CallbackContext context)
    {
        OnMenuNavigationMouse(context);
    }

    public void OnMainMenuNavigationNotMouse(InputAction.CallbackContext context)
    {
        // Set current select game object if not mouse input detected
        if (context.performed)
        {
            MainMenuUIHandler.SelectDefaultGameObject();
            Cursor.visible = false;
        }
    }

    public void OnLevelPlayerMoveUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Player.Move(Vector3.forward);
        }
    }

    public void OnLevelPlayerMoveDown(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Player.Move(Vector3.back);
        }
    }

    public void OnLevelPlayerMoveLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Player.Move(Vector3.left);
        }
    }

    public void OnLevelPlayerMoveRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Player.Move(Vector3.right);
        }
    }

    public void OnLevelExit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.OpenPauseMenu();
            PauseMenuUIHandler.SelectDefaultGameObject(); // as we open menu with keyboard
        }
    }

    public void OnPauseMenuExit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.ClosePauseMenu();
        }
    }

    public void OnPauseMenuNavigationMouse(InputAction.CallbackContext context)
    {
        OnMenuNavigationMouse(context);
    }

    public void OnPauseMenuNavigationNotMouse(InputAction.CallbackContext context)
    {
        // Set current select game object if not mouse input detected
        if (context.performed)
        {
            PauseMenuUIHandler.SelectDefaultGameObject();
            Cursor.visible = false;
        }
    }

    private void OnMenuNavigationMouse(InputAction.CallbackContext context)
    {
        // Remove current select game object if mouse move detected
        if (EventSystem.current != null &&
            EventSystem.current.currentSelectedGameObject != null &&
            context.ReadValue<Vector2>().magnitude > MouseMoveThresh)
        {
            EventSystem.current.SetSelectedGameObject(null);
            Cursor.visible = true;
        }
    }
}
