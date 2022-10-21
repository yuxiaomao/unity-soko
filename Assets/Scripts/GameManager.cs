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
        Win,
    }

    public static GameManager Instance { get; private set; }
    private static UserInputManager userInputManager;
    private static LevelManager levelManager;
    private static int currentLevel;
    private GameState currentState;
    private GameObject[] targets;
    private int totalTarget;

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
            userInputManager.OnSceneMenuLoaded();
        }
        if (scene.name.CompareTo(Constants.SceneLevel) == 0)
        {
            currentState = GameState.Level;
            levelManager.LoadLevel(LevelManager.LevelNames[currentLevel]);
            targets = GameObject.FindGameObjectsWithTag(Constants.TagTarget);
            totalTarget = targets.Length;
            userInputManager.OnSceneLevelLoaded();
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

    public static void LoadSceneMainMenu()
    {
        SceneManager.LoadScene(Constants.SceneMenu);
    }

    public static void LoadSceneLevelN(int n)
    {
        if (n >= 0 && n <= LevelManager.LevelNames.Length)
        {
            currentLevel = n;
            if (SceneManager.GetActiveScene().name.CompareTo(Constants.SceneLevel) != 0)
            {
                SceneManager.LoadScene(Constants.SceneLevel);
            }
            else
            {
                levelManager.CleanGeneratedLevels();
                levelManager.LoadLevel(LevelManager.LevelNames[n]);
            }
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
        if (Instance.currentState == GameState.Level)
        {
            Instance.StartCoroutine(Instance.UpdateScoreNextFrame());
        }
    }

    /// <summary>
    /// Wait for next physical frame (that game state is refreshed) before update target count in game manager
    /// </summary>
    private IEnumerator UpdateScoreNextFrame()
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
            AudioManager.PlaySEWin();
            Debug.Log("Win");
        }
    }
}
