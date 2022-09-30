using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject[] targets;
    private int totalTarget;

    // Start is called before the first frame update
    private void Start()
    {
        targets = GameObject.FindGameObjectsWithTag("Target");
        totalTarget = targets.Length;
    }

    /// <summary>
    /// Tell the game manager that the game state is changed
    /// </summary>
    public void ShouldUpdateGameState()
    {
        StartCoroutine(UpdateGameStateNextFrame());
    }

    /// <summary>
    /// Wait for next physical frame (that game state is refreshed) before update in game manager
    /// </summary>
    private IEnumerator UpdateGameStateNextFrame()
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
            Debug.Log("Win");
        }
    }
}
