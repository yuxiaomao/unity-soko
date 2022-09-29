using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static readonly float moveCheckDistance = 1.0f;

    // Move player to the given direction, called in FixedUpdate
    public void Move(Vector3 direction)
    {
        // Check Collision object in front of player's direction
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, direction, out hitInfo, moveCheckDistance))
        {
            // Has collision
            GameObject other = hitInfo.collider.gameObject;
            if (other.CompareTag("Wall"))
            {
                // Collision with wall, can not move
                return;
            }
            if (other.CompareTag("Box"))
            {
                // Collision with Box, try to move the Box
                if (!other.GetComponent<ObjectController>().TryMove(direction))
                {
                    // Box not moved
                    return;
                }
            }
        }

        // No collision or box moved, player move
        transform.position += direction;
    }
}
