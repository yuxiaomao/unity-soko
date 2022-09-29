using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    [SerializeField] private bool isWall = false;
    [SerializeField] private bool isBox = false;
    private static readonly float moveCheckDistance = 1.0f;

    // Return true if move can and is done, false if can not move
    public bool TryMove(Vector3 direction)
    {
        // A box can move if no wall/box in front
        if (isBox && !Physics.Raycast(transform.position, direction, moveCheckDistance))
        {
            // No collision, move on the given direction
            transform.position += direction;
            return true;
        }

        // In default case do not move
        return false;
    }
}
