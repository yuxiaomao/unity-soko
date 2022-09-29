using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {

    }

    // Move player to the given direction
    public void Move(Vector3 direction)
    {
        // Check Collision object in front of the user's direction
        // Detect if any collision wall in the given direction
        // Detect if any collision box in the given direction
        // Move if no collision
        Debug.Log("direction" + direction);
        transform.position += direction;
    }
}
