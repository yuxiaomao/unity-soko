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
    private bool lastInputIsCursor = true;
    // In level only
    private PlayerController[] players;

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

    public void UpdatePlayerReferences()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag(Constants.TagPlayer);
        players = new PlayerController[gos.Length];
        for (int i = 0; i < gos.Length; i++)
        {
            players[i] = gos[i].GetComponent<PlayerController>();
        }
    }

    // Action map call by Player Input component has 1 parameter (InputAction.CallbackContext context)

    #region OnMainMenu
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
            if (lastInputIsCursor)
            {
                lastInputIsCursor = false;
                MainMenuUIHandler.Instance.SelectDefaultGameObject();
            }
            Cursor.visible = false;
        }
    }
    #endregion

    #region OnLevel
    public void OnLevelPlayerMoveUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            LevelMoveAllPlayers(Vector3.forward);
        }
    }

    public void OnLevelPlayerMoveDown(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            LevelMoveAllPlayers(Vector3.back);
        }
    }

    public void OnLevelPlayerMoveLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            LevelMoveAllPlayers(Vector3.left);
        }
    }

    public void OnLevelPlayerMoveRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            LevelMoveAllPlayers(Vector3.right);
        }
    }

    public void OnLevelExit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.OpenPauseMenu();
            // As we open menu with keyboard
            PauseMenuUIHandler.Instance.SelectDefaultGameObject();
        }
    }
    #endregion

    #region OnPauseMenu
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
            if (lastInputIsCursor)
            {
                lastInputIsCursor = false;
                PauseMenuUIHandler.Instance.SelectDefaultGameObject();
            }
            Cursor.visible = false;
        }
    }
    #endregion

    #region OnWin
    public void OnWinExit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.OpenPauseMenu();
            // As we open menu with keyboard
            PauseMenuUIHandler.Instance.SelectDefaultGameObject();
        }
    }
    #endregion

    private void OnMenuNavigationMouse(InputAction.CallbackContext context)
    {
        // Remove current select game object if mouse move detected
        if (EventSystem.current != null &&
            EventSystem.current.currentSelectedGameObject != null &&
            context.ReadValue<Vector2>().magnitude > MouseMoveThresh)
        {
            lastInputIsCursor = true;
            EventSystem.current.SetSelectedGameObject(null);
            Cursor.visible = true;
        }
    }

    private void LevelMoveAllPlayers(Vector3 direction)
    {
        ObjectController.MoveState[] playerStates = new ObjectController.MoveState[players.Length];
        int unknownMoveCount;
        do
        {
            unknownMoveCount = 0;
            for (int i = 0; i < players.Length; i++)
            {
                if (playerStates[i] == ObjectController.MoveState.Unknown)
                {
                    playerStates[i] = players[i].TryMove(direction, true);
                    if (playerStates[i] == ObjectController.MoveState.Unknown)
                    {
                        unknownMoveCount += 1;
                    }
                }
            }
        } while (unknownMoveCount != 0);
        AudioManager.PlaySE(AudioManager.SE.Move);
        GameManager.ShouldUpdateGameState();
    }
}
