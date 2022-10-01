using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UserInputManager : MonoBehaviour
{
    private PlayerInput playerInput;

    // In level only
    private PlayerController playerController;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        playerInput = this.gameObject.GetComponent<PlayerInput>();
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
            Debug.Log("Menu Loaded! Current Action Map: " + playerInput.currentActionMap);
        }
        if (scene.name.StartsWith("Level"))
        {
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            playerInput.SwitchCurrentActionMap(playerInput.actions.FindActionMap("InLevel").name);
            playerInput.ActivateInput();
            Debug.Log("Level Loaded ! Current Action Map: " + playerInput.currentActionMap);
        }
    }

    // Action map call by Player Input component

    public void OnMenuSelect(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SceneManager.LoadScene("Level0"); // TODO Should be in GameManager?
            playerInput.DeactivateInput();
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
}
