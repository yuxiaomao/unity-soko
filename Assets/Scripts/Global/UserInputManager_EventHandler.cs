using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public partial class UserInputManager : MonoBehaviour
{
    private const float MouseMoveThresh = 0.2f;
    private bool lastInputIsCursor = true;

    // Action map call by Player Input component has 1 parameter (InputAction.CallbackContext context)

    #region MainMenu
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

    #region Level
    public void OnLevelPlayerMoveUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            LevelController.LevelMoveAllPlayers(Vector3.forward);
        }
    }

    public void OnLevelPlayerMoveDown(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            LevelController.LevelMoveAllPlayers(Vector3.back);
        }
    }

    public void OnLevelPlayerMoveLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            LevelController.LevelMoveAllPlayers(Vector3.left);
        }
    }

    public void OnLevelPlayerMoveRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            LevelController.LevelMoveAllPlayers(Vector3.right);
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

    public void OnLevelReset(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.ResetCurrentLevel();
        }
    }

    public void OnLevelUndo(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            LevelController.LevelUndoAllPlayers();
        }
    }
    #endregion

    #region PauseMenu
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

    #region Win
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

    #region Commun Private Function
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
    #endregion
}
