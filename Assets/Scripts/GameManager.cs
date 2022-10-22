#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Singleton, manage global state
/// </summary>
public class GameManager : MonoBehaviour
{
    private enum GameState
    {
        Menu,
        Level,
        PauseMenu,
        Win // TODO move win in another place, as we can open pause menu when win
    }

    public static GameManager Instance { get; private set; }
    private static UserInputManager userInputManager;
    private static LevelManager levelManager;
    private static LevelManager.Level currentLevel;
    private static GameState currentState;
    private static GameObject[] targets;
    private static int totalTarget;

    private void Awake()
    {
        // Prevent duplicate GameManager
        if (Instance != null)
        {
            // Destroy will only happends after 1st update
            Destroy(gameObject);
            // Set inactive, so Start and Update will not be called for duplication/child
            gameObject.SetActive(false);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        userInputManager = GetComponentInChildren<UserInputManager>();
        levelManager = GetComponentInChildren<LevelManager>();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Detect loaded scene and control other manager action order
        if (scene.name.CompareTo(Constants.SceneMenu) == 0)
        {
            currentState = GameState.Menu;
            userInputManager.ActivateUserInput(Constants.ActionMapMainMenu);
        }
        if (scene.name.CompareTo(Constants.SceneLevel) == 0)
        {
            currentState = GameState.Level;
            LoadLevelInternal(currentLevel);
        }
    }

    public static void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public static void OpenMainMenu()
    {
        userInputManager.DeactivateUserInput();
        SceneManager.LoadScene(Constants.SceneMenu);
    }

    public static void OpenLevel(LevelManager.Level level)
    {
        if (currentState != GameState.Level)
        {
            // Load Level scene if not in Level scene
            currentLevel = level;
            userInputManager.DeactivateUserInput();
            SceneManager.LoadScene(Constants.SceneLevel);
        } else
        {
            currentLevel = level;
            LoadLevelInternal(level);
        }
    }

    private static void LoadLevelInternal(LevelManager.Level level)
    {
        userInputManager.DeactivateUserInput();
        levelManager.CleanGeneratedLevels();
        levelManager.LoadLevelImmediate(level);
        targets = GameObject.FindGameObjectsWithTag(Constants.TagTarget);
        totalTarget = targets.Length;
        userInputManager.ActivateUserInput(Constants.ActionMapLevel);
    }

    public static void OpenPauseMenu()
    {
        if (currentState == GameState.Level)
        {
            currentState = GameState.PauseMenu;
            userInputManager.DeactivateUserInput();
            LevelOverlayUIHandler.Instance.Hide();
            PauseMenuUIHandler.Instance.Show();
            userInputManager.ActivateUserInput(Constants.ActionMapPauseMenu);
        }
    }

    public static void ClosePauseMenu()
    {
        if (currentState == GameState.PauseMenu)
        {
            currentState = GameState.Level;
            userInputManager.DeactivateUserInput();
            PauseMenuUIHandler.Instance.Hide();
            LevelOverlayUIHandler.Instance.Show();
            userInputManager.ActivateUserInput(Constants.ActionMapLevel);
        }
    }

    /// <summary>
    /// /// Check if other is a child of GameManager
    /// </summary>
    /// <param name="other"></param>
    public static void AssertIsChild(GameObject other)
    {
        Debug.Assert(other.GetComponentInParent<GameManager>() != null,
                     "This game object must be a child of GameManger game object, " +
                     "to manage scene transfer and deduplication");
    }

    /// <summary>
    /// Tell the game manager that the game state is changed
    /// </summary>
    public static void ShouldUpdateGameState()
    {
        if (currentState == GameState.Level)
        {
            Instance.StartCoroutine(UpdateScoreNextFrame());
        }
    }

    /// <summary>
    /// Wait for next physical frame (that game state is refreshed) before update target count in game manager
    /// </summary>
    private static IEnumerator UpdateScoreNextFrame()
    {
        yield return new WaitForFixedUpdate();
        // Update target count
        int withBoxTarget = 0;
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i].GetComponent<TargetController>().IsTargetWithBox())
            {
                withBoxTarget += 1;
            }
        }
        if (withBoxTarget == totalTarget)
        {
            currentState = GameState.Win;
            AudioManager.PlaySE(AudioManager.SE.Win);
            Debug.Log("Win");
        }
    }
}
