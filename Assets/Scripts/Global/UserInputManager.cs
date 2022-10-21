using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// Manage user input with Player Input component.
/// Should probably not be called by other script.
/// </summary>
public class UserInputManager : MonoBehaviour
{
    private const float MouseMoveThresh = 0.2f;
    private PlayerInput playerInput;
    // In level only
    private PlayerController playerController;

    private void Awake()
    {
        GameManager.AssertIsChild(gameObject);
        playerInput = gameObject.GetComponent<PlayerInput>();
        Debug.Assert(playerInput != null, " This script is suppose to be attached to a game object with Player Input component!");
    }

    public void OnSceneMenuLoaded()
    {
        playerInput.SwitchCurrentActionMap(playerInput.actions.FindActionMap(Constants.ActionMapMainMenu).name);
        playerInput.ActivateInput();
        Debug.Log("Menu Loaded! Current Action Map: " + playerInput.currentActionMap);
    }

    public void OnSceneLevelLoaded()
    {
        playerController = GameObject.FindGameObjectWithTag(Constants.TagPlayer).GetComponent<PlayerController>();
        playerInput.SwitchCurrentActionMap(playerInput.actions.FindActionMap(Constants.ActionMapLevel).name);
        playerInput.ActivateInput();
        Debug.Log("Level Loaded ! Current Action Map: " + playerInput.currentActionMap);
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
        // Remove current select game object if mouse move detected
        if (EventSystem.current != null &&
            EventSystem.current.currentSelectedGameObject != null &&
            context.ReadValue<Vector2>().magnitude > MouseMoveThresh)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void OnMainMenuNavigationNotMouse(InputAction.CallbackContext context)
    {
        // Set current select game object if not mouse input detected
        if (context.performed)
        {
            MainMenuUIHandler.SelectDefaultGameObject();
        }
    }

    public void OnLevelPlayerMoveUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerController.Move(Vector3.forward);
        }
    }

    public void OnLevelPlayerMoveDown(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerController.Move(Vector3.back);
        }
    }

    public void OnLevelPlayerMoveLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerController.Move(Vector3.left);
        }
    }

    public void OnLevelPlayerMoveRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerController.Move(Vector3.right);
        }
    }

    public void OnLevelExit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.LoadSceneMainMenu();
            playerInput.DeactivateInput();
        }
    }
}
