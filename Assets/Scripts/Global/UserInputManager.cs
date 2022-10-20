using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Manage user input with Player Input component.
/// Should probably not be called by other script.
/// </summary>
public class UserInputManager : MonoBehaviour
{
    private PlayerInput playerInput;

    // In level only
    private PlayerController playerController;

    private void Awake()
    {
        GameManager.AssertIsChild(gameObject);
        playerInput = gameObject.GetComponent<PlayerInput>();
        Debug.Assert(playerInput != null, " This script is suppose to be attached to a game object with Player Input component!");
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.CompareTo("Menu") == 0)
        {
            playerInput.SwitchCurrentActionMap(playerInput.actions.FindActionMap("MainMenu").name);
            playerInput.ActivateInput();
            Debug.Log("Menu Loaded! Current Action Map: " + playerInput.currentActionMap);
        }
        if (scene.name.StartsWith("Level"))
        {
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            playerInput.SwitchCurrentActionMap(playerInput.actions.FindActionMap("Level").name);
            playerInput.ActivateInput();
            Debug.Log("Level Loaded ! Current Action Map: " + playerInput.currentActionMap);
        }
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
        if (EventSystem.current.currentSelectedGameObject != null
            && context.ReadValue<Vector2>().magnitude > 0.2f)
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
