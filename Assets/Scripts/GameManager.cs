#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Singleton and manage singleton for all child gameobject,
/// if any manager singleton do something in Awake(), it should test Instance==null.
/// Manage global state.
/// </summary>
public class GameManager : MonoBehaviour
{
    private enum ScreenState
    {
        Menu,
        Level,
        PauseMenu,
    }

    private class GameState
    {
        public ScreenState screen;
        public LevelManager.Level level;
        public bool win;
    }

#if UNITY_EDITOR
    [SerializeField] private LevelManager.Level loadLevel;
#endif

    public static GameManager Instance { get; private set; }
    private static UserInputManager userInputManager;
    private static LevelManager levelManager;
    private static LevelController levelController;
    private static readonly GameState currentState = new();

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
        levelController = GetComponentInChildren<LevelController>();
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
            Debug.Log($"OnSceneLoaded: {Constants.SceneMenu}");
            currentState.screen = ScreenState.Menu;
            Instance.StartCoroutine(OnSceneLoadedLaterMenu());
        }
        if (scene.name.CompareTo(Constants.SceneLevel) == 0)
        {
            Debug.Log($"OnSceneLoaded: {Constants.SceneLevel}");
            currentState.screen = ScreenState.Level;
            Instance.StartCoroutine(OnSceneLoadedLaterLevel());
        }
    }

    private static IEnumerator OnSceneLoadedLaterMenu()
    {
        yield return new WaitWhile(() => userInputManager == null);
        userInputManager.ActivateUserInput(Constants.ActionMapMainMenu);
        MainMenuUIHandler.Instance.SelectDefaultGameObject();
    }

    private static IEnumerator OnSceneLoadedLaterLevel()
    {
        yield return new WaitWhile(() => (userInputManager == null) && (LevelManager.Instance == null));
#if UNITY_EDITOR
        // Load level indicated in editor for testing
        if (Instance.loadLevel != LevelManager.Level.None)
        {
            currentState.level = Instance.loadLevel;
        }
#endif
        if (currentState.level == LevelManager.Level.None)
        {
            currentState.level = LevelManager.Level.Level0;
        }
        LoadLevelInternal(currentState.level);
    }

    public static void ExitGame()
    {
        Debug.Log("ExitGame");
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public static void OpenMainMenu()
    {
        Debug.Log("OpenMainMenu");
        userInputManager.DeactivateUserInput();
        SceneManager.LoadScene(Constants.SceneMenu);
    }

    public static void OpenLevel(LevelManager.Level level)
    {
        Debug.Log($"OpenLevel: {level}");
        if (currentState.screen != ScreenState.Level)
        {
            // Load Level scene if not in Level scene
            currentState.level = level;
            userInputManager.DeactivateUserInput();
            SceneManager.LoadScene(Constants.SceneLevel);
        }
        else
        {
            LoadLevelInternal(level);
        }
    }

    public static void OpenNextLevel()
    {
        Debug.Log("OpenNextLevel");
        LevelManager.Level next = LevelManager.GetNextLevelOf(currentState.level);
        WinOverlayUIHandler.Instance.Hide();
        if (next == LevelManager.Level.None)
        {
            Debug.Log("All level finished, back to main menu");
            OpenMainMenu();
        }
        else
        {
            LoadLevelInternal(next);
        }
    }

    private static void LoadLevelInternal(LevelManager.Level level)
    {
        userInputManager.DeactivateUserInput();
        levelManager.CleanGeneratedLevels();
        levelManager.LoadLevelImmediate(level);
        levelController.InitCurrentLevelReferences();
        currentState.level = level;
        currentState.win = false;
        userInputManager.ActivateUserInput(Constants.ActionMapLevel);
    }

    public static void ResetCurrentLevel()
    {
        AudioManager.PlaySE(AudioManager.SE.Win);
        LoadLevelInternal(currentState.level);
    }

    public static void OpenPauseMenu()
    {
        Debug.Log("OpenPauseMenu");
        if (currentState.screen == ScreenState.Level)
        {
            currentState.screen = ScreenState.PauseMenu;
            userInputManager.DeactivateUserInput();
            LevelOverlayUIHandler.Instance.Hide();
            PauseMenuUIHandler.Instance.Show();
            userInputManager.ActivateUserInput(Constants.ActionMapPauseMenu);
        }
    }

    public static void ClosePauseMenu()
    {
        Debug.Log("ClosePauseMenu");
        if (currentState.screen == ScreenState.PauseMenu)
        {
            currentState.screen = ScreenState.Level;
            userInputManager.DeactivateUserInput();
            PauseMenuUIHandler.Instance.Hide();
            LevelOverlayUIHandler.Instance.Show();
            if (currentState.win)
            {
                WinOverlayUIHandler.Instance.SelectDefaultGameObject();
                userInputManager.ActivateUserInput(Constants.ActionMapWin);
            }
            else
            {
                userInputManager.ActivateUserInput(Constants.ActionMapLevel);
            }

        }
    }

    private static void ShowWinMessage()
    {
        userInputManager.DeactivateUserInput();
        WinOverlayUIHandler.Instance.Show();
        WinOverlayUIHandler.Instance.SelectDefaultGameObject();
        userInputManager.ActivateUserInput(Constants.ActionMapWin);
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
        if (currentState.screen == ScreenState.Level)
        {
            Instance.StartCoroutine(UpdateScoreNextFrame());
        }
    }

    /// <summary>
    /// Wait for next physical frame (that game state is refreshed)
    /// before update target count in game manager
    /// </summary>
    private static IEnumerator UpdateScoreNextFrame()
    {
        yield return new WaitForFixedUpdate();
        // Update win status
        if (levelController.LevelIsWin())
        {
            currentState.win = true;
            AudioManager.PlaySE(AudioManager.SE.Win);
            ShowWinMessage();
        }
    }
}
