using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public const string gameObjectName = "GameManager";

    private enum GameState
    {
        Menu,
        Level,
        Win,
    }

    private GameState currentState;
    private GameObject[] targets;
    private int totalTarget;


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        currentState = GameState.Menu;
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
            currentState = GameState.Menu;
        }
        if (scene.name.StartsWith("Level"))
        {
            currentState = GameState.Level;
            targets = GameObject.FindGameObjectsWithTag("Target");
            totalTarget = targets.Length;
        }
    }

    /// <summary>
    /// Tell the game manager that the game state is changed
    /// </summary>
    public void ShouldUpdateGameState()
    {
        if (currentState == GameState.Level)
        {
            StartCoroutine(UpdateScoreNextFrame());
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
        foreach (GameObject target in targets)
        {
            if (target.GetComponent<TargetController>().IsTargetWithBox())
            {
                withBoxTarget += 1;
            }
        }
        if (withBoxTarget == totalTarget)
        {
            currentState = GameState.Win;
            Debug.Log("Win");
        }
    }
}
